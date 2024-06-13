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
using Microsoft.Data.SqlClient;

namespace Mensajeria.Service.EventHandler.Handlers.Incidencias
{
    public class IncidenciaDeleteEventHandler : IRequestHandler<IncidenciaDeleteCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaDeleteEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(IncidenciaDeleteCommand request, CancellationToken cancellationToken)
        {
            var incidencia = _context.Incidencias.SingleOrDefault(i => i.Id == request.Id);

            try
            {
                if (incidencia.Acta != null)
                {
                    eliminaArchivoActual(request.Anio, request.Mes, request.Folio, incidencia.Acta, request.TipoIncidencia);
                }

                incidencia.FechaEliminacion = DateTime.Now;
                await _context.SaveChangesAsync();
                return incidencia.Id;
            }
            catch (Exception ex)
            {
                string message = ex.ToString();
                return -1;
            }
        }

        public bool eliminaArchivoActual(int anio, string mes, string folio, string archivo, string tipoIncidencia)
        {
            string newPath = Directory.GetCurrentDirectory() + "\\Entregables\\" + anio + "" + "\\" + mes + "\\" + folio + "\\Actas " + tipoIncidencia;

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
