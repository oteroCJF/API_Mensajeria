using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Respuestas;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Respuestas
{
    public interface IRespuestasQueryService
    {
        Task<List<RespuestaDto>> GetAllRespuestasByAnioAsync(int anio);
        Task<List<RespuestaDto>> GetRespuestasByCedulaAsync(int cedula);
        Task<decimal> GetTotalPenasDeductivas(int cedula);
        public bool GetDeductivasByCedula(int cedula, List<RespuestaDto> respuestas);
    }

    public class RespuestaQueryService : IRespuestasQueryService
    {
        private readonly ApplicationDbContext _context;

        public RespuestaQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<RespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            try
            {
                var cedulas = await _context.CedulaEvaluacion.Where(c => c.Anio == anio).Select(c => c.Id).ToListAsync();
                var respuestas = await _context.Respuestas.Where(r => cedulas.Contains(r.CedulaEvaluacionId))
                                .ToListAsync();
                return respuestas.MapTo<List<RespuestaDto>>();

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<RespuestaDto>> GetRespuestasByCedulaAsync(int cedula)
        {
            try
            {
                var respuestas = await _context.Respuestas.Where(r => r.CedulaEvaluacionId == cedula)
                                                          .OrderBy(r => r.Pregunta)
                                                          .ToListAsync();

                return respuestas.MapTo<List<RespuestaDto>>();

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<decimal> GetTotalPenasDeductivas(int cedula)
        {
            try
            {
                var totalPD = await _context.Respuestas.Where(r => r.CedulaEvaluacionId == cedula).Select(r => r.MontoPenalizacion).SumAsync();
                var tot = await _context.Respuestas.Where(r => r.CedulaEvaluacionId == cedula).ToListAsync();

                return Convert.ToDecimal(totalPD);

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return 0;
            }
        }

        public bool GetDeductivasByCedula(int id, List<RespuestaDto> respuestas)
        {
            try
            {
                var cedula = _context.CedulaEvaluacion.SingleOrDefault(c => c.Id == id);

                var preguntas = _context.CuestionarioMensual
                    .Where(cm => cm.Anio == cedula.Anio && cm.MesId == cedula.MesId 
                                && cm.ContratoId == cedula.ContratoId && cm.Tipo.Equals("Deductiva"))
                                .Select(cm => cm.Consecutivo).ToList();

                var m = respuestas.Where(r => r.CedulaEvaluacionId == cedula.Id && preguntas.Contains(r.Pregunta)).Sum(f => f.MontoPenalizacion);

                return m > 0 ? true:false;

            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

    }
}
