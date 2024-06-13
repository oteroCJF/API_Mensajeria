using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Entregables;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using Service.Common.Collection;
using Service.Common.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Entregables
{
    public interface IEntregableQueryService
    {
        Task<List<EntregableDto>> GetAllEntregablesAsync();
        Task<List<EntregableDto>> GetEntregablesByCedula(int cedula);
        Task<EntregableDto> GetEntregableById(int entregable);
        Task<DataCollection<EntregableDto>> GetEntregablesValidados();
    }

    public class EntregableQueryService : IEntregableQueryService
    {
        private readonly ApplicationDbContext _context;

        public EntregableQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<EntregableDto>> GetAllEntregablesAsync()
        {
            var entregables = await _context.Entregables.Where( e=> !e.FechaEliminacion.HasValue).ToListAsync();

            return entregables.MapTo<List<EntregableDto>>();
        }

        public async Task<List<EntregableDto>> GetEntregablesByCedula(int cedula)
        {
            try
            {
                var entregables = await _context.Entregables
                    .Where(x => x.CedulaEvaluacionId == cedula && !x.FechaEliminacion.HasValue)
                    .ToListAsync();

                return entregables.MapTo<List<EntregableDto>>();
            }
            catch (Exception ex)
            { 
                string message = ex.Message;
                return null;
            }
        }

        public async Task<EntregableDto> GetEntregableById(int entregable)
        {
            var entregables = await _context.Entregables.SingleOrDefaultAsync(x => x.Id == entregable);

            return entregables.MapTo<EntregableDto>();
        }

        public async Task<DataCollection<EntregableDto>> GetEntregablesValidados()
        {
            try
            {
                var total = _context.Entregables.Where(e => e.Validado == true && !e.FechaEliminacion.HasValue).Count();

                var entregables = await _context.Entregables
                    .Where(e => e.Validado == true && !e.FechaEliminacion.HasValue)
                    .Select(e => new EntregableDto
                    {
                        Id = e.Id,
                        CedulaEvaluacionId = e.CedulaEvaluacionId,
                        EntregableId = e.EntregableId,
                        EstatusId = e.EstatusId,
                        Validado = e.Validado,
                    })
                    .GetPagedAsync(1, total);

                return entregables.MapTo<DataCollection<EntregableDto>>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
    }
}
