using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.Entregables;
using Mensajeria.Service.EventHandler.Commands.Incidencias;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using System.IO;
using Mensajeria.Domain.DEntregables;
using System.Text.RegularExpressions;
using Mensajeria.Service.EventHandler.Commands.LogEntregables;

namespace Mensajeria.Service.EventHandler.Handlers.Entregables
{
    public class ESREntregableUpdateEventHandler : IRequestHandler<ESREntregableUpdateCommand, Entregable>
    {
        private readonly ApplicationDbContext _context;

        public ESREntregableUpdateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Entregable> Handle(ESREntregableUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                Entregable entregable = _context.Entregables.Where(e => e.Id == request.Id && !e.FechaEliminacion.HasValue).FirstOrDefault();

                entregable.UsuarioId = request.UsuarioId;
                entregable.EstatusId = request.EstatusId;
                if (request.Aprobada) {
                    entregable.FechaEliminacion = DateTime.Now;
                }
                else
                {
                    entregable.FechaEliminacion = null;
                }

                await _context.SaveChangesAsync();

                return entregable;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
