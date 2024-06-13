using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Mensajeria.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Linq;

namespace Mensajeria.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaCreateEventHandler : IRequestHandler<IncidenciaCreateCommand, Incidencia>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Incidencia> Handle(IncidenciaCreateCommand request, CancellationToken cancellationToken)
        {
            var incidencia = new Incidencia();

            try
            {
                incidencia = await GetModelIncidencia(request);
                await _context.AddAsync(incidencia);
                await _context.SaveChangesAsync();

                return incidencia;
            }
            catch(Exception ex)
            {
                string message = ex.ToString();
                return incidencia;
            }
        }

        private async Task<Incidencia> GetModelIncidencia(IncidenciaCreateCommand request)
        {
            string newDate = DateTime.Now.ToString("yyyyMMddHHmmss");

            if (request.Acta != null)
            {
                await guardaArchivo(request.Acta, request.Anio, request.Mes, request.Folio, newDate, request.TipoIncidencia, "Acta MP");
            }

            if (request.Escrito != null)
            {
                await guardaArchivo(request.Escrito, request.Anio, request.Mes, request.Folio, newDate, request.TipoIncidencia, "Escrito");
            }

            if(request.Comprobante != null)
            {
                await guardaArchivo(request.Comprobante, request.Anio, request.Mes, request.Folio, newDate, request.TipoIncidencia, "Comprobante");
            }

            var incidencia = new Incidencia 
            { 
                UsuarioId = request.UsuarioId,
                IncidenciaId = request.IncidenciaId,
                IndemnizacionId = request.IndemnizacionId,
                CedulaEvaluacionId = request.CedulaEvaluacionId,
                Pregunta = request.Pregunta,
                FechaProgramada = request.FechaProgramada,
                FechaEntrega = request.FechaEntrega,
                NumeroGuia = request.NumeroGuia,
                CodigoRastreo = request.CodigoRastreo,
                Acuse = request.Acuse,
                TotalAcuses = request.TotalAcuses,
                TipoServicio = request.TipoServicio,
                Sobrepeso = request.Sobrepeso,
                Acta = request.Acta != null ? newDate + "_" + request.Acta.FileName : "",
                Escrito = request.Escrito != null ? newDate + "_" + request.Escrito.FileName:"",
                Comprobante = request.Comprobante != null ? newDate + "_" + request.Comprobante.FileName:"",
                Observaciones = request.Observaciones,
                FechaCreacion = DateTime.Now,
            };

            return incidencia;
        }

        public async Task<bool> guardaArchivo(IFormFile archivo, int anio, string mes, string folio, string fecha, string tipoIncidencia, string tipo)
        {
            long size = archivo.Length;

            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + anio + "" + "\\" + mes + "\\" + folio+"\\Actas "+tipoIncidencia+"\\"+tipo;
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
        
    }
}
