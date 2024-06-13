using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Incidencias;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Incidencias
{
    public interface ISoportePagoQueryService
    {
        Task<List<SoportePagoDto>> GetSoportePagoByCedula(int cedula);
        Task<List<SoportePagoDto>> GetGuiasPendientes(int cedula);
        Task<List<SoportePagoDto>> GetSoportePagoByCliente(string cliente);
        Task<SoportePagoDto> GetSoportePagoById(int soporte);
    }

    public class SoportePagoQueryService : ISoportePagoQueryService
    {
        private readonly ApplicationDbContext _context;

        public SoportePagoQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<SoportePagoDto>> GetSoportePagoByCedula(int cedula)
        {
            var soporte = await _context.SoportePago.Where(sp => !sp.FechaEliminacion.HasValue && sp.CedulaEvaluacionId == cedula).ToListAsync();

            return soporte.MapTo<List<SoportePagoDto>>();
        }

        public async Task<List<SoportePagoDto>> GetGuiasPendientes(int cedula)
        {
            try
            {

                var guias = await _context.Incidencias.Where(sp => !sp.FechaEliminacion.HasValue &&
                                                         sp.CedulaEvaluacionId == cedula).Select(i => i.NumeroGuia).ToListAsync();

                var soporte = await _context.SoportePago.Where(sp => sp.CedulaEvaluacionId == cedula && 
                                                                     !guias.Contains(sp.NumeroGuia) &&
                                                                     sp.FechaEntrega.Year == 1990 && 
                                                                     sp.FechaEntrega.Month == 1 &&
                                                                     sp.FechaEntrega.Day == 1).ToListAsync();

                return soporte.MapTo<List<SoportePagoDto>>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<SoportePagoDto>> GetSoportePagoByCliente(string cliente)
        {
            var result = await _context.SoportePago.Where(sp => sp.Cliente.Equals(cliente)).ToListAsync();

            return result.MapTo<List<SoportePagoDto>>();
        }

        public async Task<SoportePagoDto> GetSoportePagoById(int soporte)
        {
            var result = await _context.SoportePago.SingleOrDefaultAsync(sp => sp.Id == soporte);

            return result.MapTo<SoportePagoDto>();
        }
    }
}
