using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.LogCedulas;
using Mensajeria.Service.Queries.DTOs.LogOficios;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.LogOficios
{
    public interface ILogOficioQueryService
    {
        Task<List<LogOficioDto>> GetHistorialOficio(int oficio);
    }

    public class LogOficioQueryService : ILogOficioQueryService
    {
        private readonly ApplicationDbContext _context;

        public LogOficioQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<LogOficioDto>> GetHistorialOficio(int oficio)
        {
            var historial = await _context.LogOficios.Where(h => h.OficioId == oficio).OrderByDescending(h => h.FechaCreacion).ToListAsync();

            return historial.MapTo<List<LogOficioDto>>();
        }
    }
}
