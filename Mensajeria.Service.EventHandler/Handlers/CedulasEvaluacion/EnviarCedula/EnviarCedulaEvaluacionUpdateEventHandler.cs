using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System.Data;
using Mensajeria.Persistence.Database;
using Mensajeria.Domain.DCedulaEvaluacion;
using Mensajeria.Domain.DFacturas;
using Mensajeria.Domain.DIncidencias;
using Mensajeria.Domain.DCuestionario;
using Mensajeria.Service.EventHandler.Commands.CedulasEvaluacion;

namespace Mensajeria.Service.EventHandler.Handlers.CedulasEvaluacion
{
    public class EnviarCedulaEvaluacionUpdateEventHandler : IRequestHandler<EnviarCedulaEvaluacionUpdateCommand, CedulaEvaluacion>
    {
        private readonly ApplicationDbContext _context;

        public EnviarCedulaEvaluacionUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CedulaEvaluacion> Handle(EnviarCedulaEvaluacionUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CedulaEvaluacion cedula = _context.CedulaEvaluacion.FirstOrDefault(c => c.Id == request.Id);

                if (request.Calcula)
                {
                    List<Factura> facturas = _context.Facturas
                                                                   .Where(f => f.RepositorioId == request.RepositorioId &&
                                                                               f.InmuebleId == cedula.InmuebleId && f.Tipo.Equals("Factura")
                                                                               && f.Facturacion.Equals("Mensual"))
                                                                   .ToList();
                    
                    List<Factura> notasCredito = _context.Facturas
                                                               .Where(f => f.RepositorioId == request.RepositorioId &&
                                                                           f.InmuebleId == cedula.InmuebleId && f.Tipo.Equals("NC")
                                                                           && f.Facturacion.Equals("Mensual"))
                                                               .ToList();

                    List<CuestionarioMensual> cuestionarioMensual = _context.CuestionarioMensual
                                                                .Where(cm => cm.Anio == cedula.Anio && cm.MesId == cedula.MesId && cm.ContratoId == cedula.ContratoId)
                                                                .ToList();

                    var incidencias = await CalculaPDIncidencias(request.Id, cuestionarioMensual, request.Penalizacion, request.Indemnizaciones, 
                                                                 facturas, request.UMA);
                    var respuestas = await Obtienetotales(request.Id, cuestionarioMensual);
                    var calificacion = await GetCalificacionCedula(request.Id, cuestionarioMensual);

                    cedula.EstatusId = request.EstatusId;
                    if (calificacion < 10)
                    {
                        // string calif = calificacion.ToString().Substring(0, 3);
                        string calif = (Math.Round(calificacion, 1)).ToString();
                        cedula.Calificacion = Convert.ToDouble(calif);
                    }
                    else
                    {
                        cedula.Calificacion = (double)calificacion;
                    }

                    if (Convert.ToDecimal(cedula.Calificacion) < Convert.ToDecimal(8))
                    {
                        //cedula.Penalizacion = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01)) / calificacion;

                        cedula.Penalizacion = (Convert.ToDecimal(facturas.Sum(f => f.Subtotal)) * Convert.ToDecimal(0.01)) / Convert.ToDecimal(cedula.Calificacion);
                        cedula.Penalizacion = Math.Round(cedula.Penalizacion, 2);
                    }
                    else
                    {
                        cedula.Penalizacion = 0;
                    }
                    
                    cedula.FechaActualizacion = request.FechaActualizacion;
                    
                    await _context.SaveChangesAsync();

                    foreach (var fac in facturas)
                    {
                        fac.EstatusId = request.EFacturaId;

                        await _context.SaveChangesAsync();
                    }

                    foreach (var fac in notasCredito)
                    {
                        fac.EstatusId = request.ENotaCreditoId;

                        await _context.SaveChangesAsync();
                    }
                }

                return cedula;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }        

        private async Task<List<Incidencia>> CalculaPDIncidencias(int cedula, List<CuestionarioMensual> cuestionario, 
                                                                   List<ServicioContratoDto> servicio, List<CTIndemnizacionDto> indemnizaciones,
                                                                   List<Factura> facturas, decimal UMA)
        {
            var incidencias = new List<Incidencia>();
            decimal costoGN = servicio.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("GNacionales")).PrecioUnitario;
            decimal costoGI = servicio.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("GInternacionales")).PrecioUnitario;
            decimal costoSN = servicio.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("SobrepesoN")).PrecioUnitario;
            decimal costoSI = servicio.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("SobrepesoI")).PrecioUnitario;
            decimal costoAN = servicio.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("ARNacional")).PrecioUnitario;
            decimal costoAI = servicio.SingleOrDefault(sc => sc.Servicio.Abreviacion.Contains("ARInternacional")).PrecioUnitario;
            decimal montoP = 0;
            decimal sobrepesoI = 0;

            foreach (var cm in cuestionario)
            {
                incidencias = _context.Incidencias.Where(i => i.Pregunta == cm.Consecutivo && i.CedulaEvaluacionId == cedula 
                                                                && !i.FechaEliminacion.HasValue).ToList();
                foreach (var inc in incidencias)
                {
                    montoP = 0;
                    DateTime fechaP = (DateTime)inc.FechaProgramada;
                    DateTime fechaE = (DateTime)inc.FechaEntrega;
                    var tipoServicio = inc.TipoServicio;
                    TimeSpan diffDate = fechaE - fechaP;
                    var diasNaturales = diffDate.Days;
                    if (cm.Formula.Contains("CTG"))
                    {
                        if (inc.Sobrepeso != 0 && tipoServicio.Contains("Nacional") && tipoServicio.Contains("Sobrepeso"))
                        {
                            montoP = costoGN + (costoSN * inc.Sobrepeso);
                            montoP = (inc.Acuse.Equals("SI") ? montoP + costoAN : montoP);
                        }
                        else if (inc.Sobrepeso != 0 && tipoServicio.Contains("Internacional") && tipoServicio.Contains("Sobrepeso"))
                        {
                            montoP = costoGI + (costoSI * inc.Sobrepeso);
                            montoP = (inc.Acuse.Equals("SI") ? montoP + costoAI : montoP);
                        }
                        else if (inc.Sobrepeso == 0 && tipoServicio.Contains("Nacional"))
                        {
                            montoP = costoGN;
                            montoP = (inc.Acuse.Equals("SI") ? montoP + costoAN : montoP);
                        }
                        else if (inc.Sobrepeso == 0 && tipoServicio.Contains("Internacional"))
                        {
                            montoP = costoGI;
                            montoP = (inc.Acuse.Equals("SI") ? montoP + costoAI : montoP);
                        }

                        montoP = (montoP * cm.Porcentaje) * diasNaturales;
                    }
                    else if (cm.Formula.Contains("CTA"))
                    {
                        montoP = (costoAN * cm.Porcentaje) * inc.TotalAcuses;
                        montoP = montoP * diasNaturales;
                    }
                    else if (cm.Formula.Contains("NPNG") || cm.Formula.Contains("NPE"))
                    {
                        montoP = cm.Porcentaje * 1;
                    }
                    else if (cm.Formula.Contains("NPNG") || cm.Formula.Contains("NPE"))
                    {
                        montoP = cm.Porcentaje * 1;
                    }
                    else if (cm.Formula.Contains("CFM"))
                    {
                        montoP = (decimal)facturas.Sum(f => f.Subtotal);
                        montoP = montoP * cm.Porcentaje;
                    }

                   /* if (inc.IndemnizacionId != 0)
                    {
                        if (inc.Sobrepeso != 0)
                        {
                            
                            sobrepesoI = (Math.Round(inc.Sobrepeso, MidpointRounding.AwayFromZero) * 30)*UMA;
                        }
                        else
                        {
                            montoP += Convert.ToInt32(indemnizaciones.Single(iz => iz.Id == inc.IndemnizacionId).UMAS) * UMA;
                        }
                    }*/

                    inc.Penalizable = montoP != 0 ? true : false;
                    inc.MontoPenalizacion = montoP;

                    await _context.SaveChangesAsync();
                }
            }
            return incidencias;
        }
        
        private async Task<List<RespuestaEvaluacion>> Obtienetotales(int cedula, List<CuestionarioMensual> cuestionario)
        {
            var incidencias = new List<Incidencia>();

            foreach (var cm in cuestionario)
            {
                incidencias = _context.Incidencias.Where(i => i.Pregunta == cm.Consecutivo && i.CedulaEvaluacionId == cedula
                                                            && !i.FechaEliminacion.HasValue).ToList();
                var respuesta = _context.Respuestas.SingleOrDefault(r => r.CedulaEvaluacionId == cedula && r.Pregunta == cm.Consecutivo);

                if (!respuesta.Detalles.Equals("N/A")) {
                    respuesta.Detalles = incidencias.Count() + "";
                }
                respuesta.Penalizable = (incidencias.Sum(i => i.MontoPenalizacion) != 0 ? true : false);
                respuesta.MontoPenalizacion = (incidencias.Sum(i => i.MontoPenalizacion) != 0 ? incidencias.Sum(i => i.MontoPenalizacion) : 0);

                await _context.SaveChangesAsync();
            }
            var respuestas = _context.Respuestas.ToList();

            return respuestas;
        }

        private async Task<decimal> GetCalificacionCedula(int cedula, List<CuestionarioMensual> cuestionario)
        {
            decimal calificacion = 0;
            decimal ponderacion = 0;
            bool calidad = true;
            var incidencias = 0;

            var respuestas = _context.Respuestas.Where(r => r.CedulaEvaluacionId == cedula).ToList();

            foreach (var rs in respuestas)
            {
                var cm = cuestionario.Single(c => c.Consecutivo == rs.Pregunta);
                if (cm.ACLRS == rs.Respuesta && !rs.Detalles.Equals("N/A"))
                {
                    calidad = false;
                    incidencias = _context.Incidencias.Where(i => i.CedulaEvaluacionId == cedula && i.Pregunta == cm.Consecutivo
                                                            && !i.FechaEliminacion.HasValue).Count();
                    
                    //AQUI SE LLEVAN A CABO LAS MODIFICACIONES PARA PREGUNTAS INDIV DE MENSAJERÍA
                    if (cm.Formula.Contains("CTG"))
                    {
                        ponderacion = Convert.ToDecimal(incidencias) * Convert.ToDecimal(0.01);
                        ponderacion = Convert.ToDecimal(cm.Ponderacion) - Convert.ToDecimal(ponderacion);
                    }
                    else 
                    {
                        ponderacion = Convert.ToDecimal(cm.Ponderacion) / Convert.ToDecimal(2);
                    }

                    calificacion += ponderacion;

                    rs.Detalles = incidencias+"";
                    rs.Penalizable = true;
                    rs.MontoPenalizacion = _context.Incidencias.Where(i => i.CedulaEvaluacionId == cedula && 
                                                                      i.Pregunta == cm.Consecutivo && 
                                                                      !i.FechaEliminacion.HasValue).Sum(i => i.MontoPenalizacion);

                    await _context.SaveChangesAsync();
                }
                else
                {
                    calificacion += Convert.ToDecimal(cm.Ponderacion);
                    rs.Penalizable = false;
                }
            }

            calificacion = Convert.ToDecimal(calificacion / respuestas.Count());

             return calidad ? calificacion + (decimal)1 : calificacion;
        }
    }
}
