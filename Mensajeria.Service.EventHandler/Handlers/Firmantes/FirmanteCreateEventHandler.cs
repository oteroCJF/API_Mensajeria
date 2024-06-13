using Mensajeria.Persistence.Database;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Mensajeria.Service.EventHandler.Commands.Firmantes;
using Mensajeria.Domain.DFirmantes;

namespace Mensajeria.Service.EventHandler.Handlers.Firmantes
{
    public class FirmanteCreateEventHandler : IRequestHandler<FirmantesCreateCommand, Firmante>
    {
        private readonly ApplicationDbContext _context;

        public FirmanteCreateEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Firmante> Handle(FirmantesCreateCommand firmantes, CancellationToken cancellationToken)
        {
            try
            {
                var firmante = new Firmante
                {
                    UsuarioId = firmantes.UsuarioId,
                    Tipo = firmantes.Tipo,
                    InmuebleId = firmantes.InmuebleId,
                    Escolaridad = firmantes.Escolaridad,
                    FechaCreacion = DateTime.Now
                };

                await _context.AddAsync(firmante);
                await _context.SaveChangesAsync();

                return firmante;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }            
        }
    }
}
