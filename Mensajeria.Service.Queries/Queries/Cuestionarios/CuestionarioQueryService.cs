using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Cuestionario;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Cuestionarios
{
    public interface ICuestionariosQueryService
    {
        Task<List<CuestionarioDto>> GetAllPreguntasAsync();
        Task<List<CuestionarioMensualDto>> GetCuestionarioMensualAsync(int anio, int mes, int contrato);
        Task<List<CuestionarioMensualDto>> GetPreguntasDeductiva(int anio, int mes, int contrato);
        Task<CuestionarioDto> GetPreguntaByIdAsync(int pregunta);
    }

    public class CuestionarioQueryService : ICuestionariosQueryService
    {
        private readonly ApplicationDbContext _context;

        public CuestionarioQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CuestionarioDto>> GetAllPreguntasAsync()
        {
            var preguntas = await _context.Cuestionarios.ToListAsync();

            return preguntas.MapTo<List<CuestionarioDto>>();
        }

        public async Task<List<CuestionarioMensualDto>> GetCuestionarioMensualAsync(int anio, int mes, int contrato)
        {
            var preguntas = await _context.CuestionarioMensual.Where(x => x.Anio == anio && x.MesId == mes && x.ContratoId == contrato).ToListAsync();

            return preguntas.MapTo<List<CuestionarioMensualDto>>();
        }
        
        public async Task<List<CuestionarioMensualDto>> GetPreguntasDeductiva(int anio, int mes, int contrato)
        {
            var preguntas = await _context.CuestionarioMensual.Where(x => x.Anio == anio && x.MesId == mes 
                                                                        && x.ContratoId == contrato &&
                                                                        x.Tipo.Equals("Deductiva")).ToListAsync();

            return preguntas.MapTo<List<CuestionarioMensualDto>>();
        }

        public async Task<CuestionarioDto> GetPreguntaByIdAsync(int pregunta)
        {
            return (await _context.Cuestionarios.SingleAsync(x => x.Id == pregunta)).MapTo<CuestionarioDto>();
        }
    }
}
