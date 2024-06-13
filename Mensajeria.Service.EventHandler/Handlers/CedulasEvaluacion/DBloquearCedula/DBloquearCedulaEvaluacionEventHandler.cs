using MediatR;
using Mensajeria.Domain.DCedulaEvaluacion;
using Mensajeria.Domain.DFacturas;
using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.CedulasEvaluacion.DBloquearCedula;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mensajeria.Service.EventHandler.Handlers.CedulasEvaluacion.BloquearCedula
{
    public class DBloquearCedulaEvaluacionEventHandler : IRequestHandler<DBloquearCedulaUpdateCommand, CedulaEvaluacion>
    {
        private readonly ApplicationDbContext _context;

        public DBloquearCedulaEvaluacionEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<CedulaEvaluacion> Handle(DBloquearCedulaUpdateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                CedulaEvaluacion cedula = _context.CedulaEvaluacion.FirstOrDefault(c => c.Id == request.Id);

                List<Factura> facturas = _context.Facturas
                                                               .Where(f => f.RepositorioId == request.RepositorioId &&
                                                                           f.InmuebleId == cedula.InmuebleId && f.Tipo.Equals("Factura")
                                                                           && f.Facturacion.Equals("Mensual"))
                                                               .ToList();

                cedula.EstatusId = request.EstatusId;
                cedula.Bloqueada = request.Bloqueada;
                cedula.FechaActualizacion = request.FechaActualizacion;
                await _context.SaveChangesAsync();

                foreach (var fac in facturas)
                {
                    fac.EstatusId = request.EFacturaId;
                    fac.FechaActualizacion = request.FechaActualizacion;

                    await _context.SaveChangesAsync();
                }

                return cedula;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
