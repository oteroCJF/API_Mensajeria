using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Mensajeria.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.IO;

namespace Mensajeria.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaUpdateEventHandler : IRequestHandler<IncidenciaUpdateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaUpdateCommand request, CancellationToken cancellationToken)
        {
            var incidencia = _context.Incidencias.SingleOrDefault(i => i.Id == request.Id);

            if (request.Acta != null)
            {
                eliminaArchivoActual(request.Anio, request.Mes, request.Folio, incidencia.Acta, request.TipoIncidencia, "Acta MP");
            }

            if (request.Escrito != null)
            {
                eliminaArchivoActual(request.Anio, request.Mes, request.Folio, incidencia.Escrito, request.TipoIncidencia, "Escrito");
            }
            
            if (request.Comprobante != null)
            {
                eliminaArchivoActual(request.Anio, request.Mes, request.Folio, incidencia.Comprobante, request.TipoIncidencia, "Comprobante");
            }           

            try
            {
                incidencia = await GetModelIncidencia(request, incidencia);
                await _context.SaveChangesAsync();

                return incidencia;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                return incidencia;
            }
        }

        private async Task<Incidencia> GetModelIncidencia(IncidenciaUpdateCommand request, Incidencia incidencia)
        {
            string newDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (request.Acta != null)
            {
                await guardaArchivo(request.Acta, request.Anio, request.Mes, request.Folio, newDate, request.TipoIncidencia, "Acta MP");
                incidencia.Acta = newDate + "_" + request.Acta.FileName;
            }

            if (request.Escrito != null)
            {
                await guardaArchivo(request.Escrito, request.Anio, request.Mes, request.Folio, newDate, request.TipoIncidencia, "Escrito");
                incidencia.Escrito = newDate + "_" + request.Escrito.FileName;
            }
            
            if (request.Comprobante != null)
            {
                await guardaArchivo(request.Comprobante, request.Anio, request.Mes, request.Folio, newDate, request.TipoIncidencia, "Comprobante");
                incidencia.Comprobante = newDate + "_" + request.Comprobante.FileName;
            }

            incidencia.UsuarioId = request.UsuarioId;
            incidencia.IncidenciaId = request.IncidenciaId;
            incidencia.IndemnizacionId = request.IndemnizacionId;
            incidencia.CedulaEvaluacionId = request.CedulaEvaluacionId;
            incidencia.Pregunta = request.Pregunta;
            incidencia.FechaProgramada = request.FechaProgramada;
            incidencia.FechaEntrega = request.FechaEntrega;
            incidencia.NumeroGuia = request.NumeroGuia;
            incidencia.CodigoRastreo = request.CodigoRastreo;
            incidencia.Acuse = request.Acuse;
            incidencia.TotalAcuses = request.TotalAcuses;
            incidencia.TipoServicio = request.TipoServicio;
            incidencia.Sobrepeso = request.Sobrepeso;
            incidencia.Observaciones = request.Observaciones;
            incidencia.FechaActualizacion = DateTime.Now;

            return incidencia;
        }

        public async Task<bool> guardaArchivo(IFormFile archivo, int anio, string mes, string folio, string fecha, string tipoIncidencia, string tipo)
        {
            long size = archivo.Length;

            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + anio + "" + "\\" + mes + "\\" + folio + "\\Actas " + tipoIncidencia+"\\"+tipo;
            if (!Directory.Exists(newPath))
            {
                Directory.CreateDirectory(newPath);
            }

            using (var stream = new FileStream(newPath + "\\" + fecha + "_" + archivo.FileName, FileMode.Create))
            {
                try
                {
                    await archivo.CopyToAsync(stream);
                    return true;
                }
                catch (Exception ex)
                {
                    string msg = ex.Message.ToString();
                    return false;
                }
            }
        }

        public bool eliminaArchivoActual(int anio, string mes, string folio, string archivo, string tipoIncidencia, string tipo)
        {
            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + anio + "" + "\\" + mes + "\\" + folio + "\\Actas " + tipoIncidencia+"\\"+tipo;

            newPath += "\\" + archivo;

            try
            {
                File.Delete(newPath);
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message.ToString();
                return false;
            }
        }
    }
}
