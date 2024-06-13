using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Facturas;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Repositorios
{
    public interface IRepositorioQueryService
    {
        Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio);

        Task<RepositorioDto> GetRepositorioByIdAsync(int Repositorio);

        Task<RepositorioDto> GetRepositorioByAMCAsync(int anio, int mes, int contrato);
    }

    public class RepositorioQueryService : IRepositorioQueryService
    {
        private readonly ApplicationDbContext _context;

        public RepositorioQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RepositorioDto>> GetAllRepositoriosAsync(int anio)
        {
            try
            {
                var collection = await _context.Repositorios.Where(x => x.Anio == anio).OrderBy(x => x.MesId).ToListAsync();

                return collection.MapTo<List<RepositorioDto>>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<RepositorioDto> GetRepositorioByAMCAsync(int anio, int mes, int contrato)
        {
            try
            {
                var collection = await _context.Repositorios.SingleOrDefaultAsync(x => x.Anio == anio && x.MesId == mes && x.ContratoId == contrato);

                return collection.MapTo<RepositorioDto>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        
        public async Task<RepositorioDto> GetRepositorioByIdAsync(int repositorio)
        {
            try
            {
                var collection = await _context.Repositorios.SingleOrDefaultAsync(x => x.Id == repositorio);

                return collection.MapTo<RepositorioDto>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

    }
}
