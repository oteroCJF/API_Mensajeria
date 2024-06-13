using Mensajeria.Persistence.Database;
using MediatR;
using System;
using System.Threading.Tasks;
using System.Threading;
using Mensajeria.Service.EventHandler.Commands.Convenios;
using Microsoft.EntityFrameworkCore;
using Mensajeria.Domain.DContratos;

namespace Mensajeria.Service.EventHandler.Handlers.Convenios
{
    internal class ConvenioDeleteEventHandler : IRequestHandler<ConvenioDeleteCommand, int>
    {
        private readonly ApplicationDbContext _context;

        public ConvenioDeleteEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(ConvenioDeleteCommand request, CancellationToken cancellationToken)
        {
            Convenio convenio = await _context.Convenio.SingleOrDefaultAsync(c => c.Id == request.Id);

            convenio.FechaEliminacion = DateTime.Now;

            try
            {
                await _context.SaveChangesAsync();
                return 200;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 500;
            }
        }
    }
}
