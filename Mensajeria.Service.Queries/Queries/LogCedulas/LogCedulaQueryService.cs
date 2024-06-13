using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.LogCedulas;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.LogCedulas
{
    public interface ILogCedulasQueryService
    {
        Task<List<LogCedulaDto>> GetHistorialCedula(int cedula);
    }

    public class LogCedulaQueryService : ILogCedulasQueryService
    {
        private readonly ApplicationDbContext _context;

        public LogCedulaQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LogCedulaDto>> GetHistorialCedula(int cedula)
        {
            var historial = await _context.LogCedulas.Where(h => h.CedulaEvaluacionId == cedula).OrderByDescending(h => h.FechaCreacion).ToListAsync();

            return historial.MapTo<List<LogCedulaDto>>();
        }
    }
}
