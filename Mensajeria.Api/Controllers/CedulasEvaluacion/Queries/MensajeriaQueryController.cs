using Mensajeria.Service.Queries.DTOs.CedulaEvaluacion;
using Mensajeria.Service.Queries.Queries.Incidencias;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mensajeria.Service.Queries.Queries.CedulasEvaluacion;
using Service.Common.Collection;
using Mensajeria.Service.Queries.Queries.Respuestas;
using System.Linq;

namespace Mensajeria.Api.Controllers.CedulasEvaluacion.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/cedulaEvaluacion")]
    public class MensajeriaQueryController : ControllerBase
    {
        private readonly IMensajeriaQueryService _cedula;
        private readonly IRespuestasQueryService _respuestas;

        public MensajeriaQueryController(IMensajeriaQueryService limpieza, IRespuestasQueryService respuestas)
        {
            _cedula = limpieza;
            _respuestas = respuestas;
        }

        [HttpGet]
        public async Task<List<CedulaEvaluacionDto>> GetAllCedulasEvaluacion()
        {
            return await _cedula.GetAllCedulasAsync();
        }

        [Route("getCedulasByAnio/{anio}")]
        [HttpGet]
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnio(int anio)
        {
            var result = await _cedula.GetCedulaEvaluacionByAnio(anio);
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            if (result != null)
            {
                var cedulas = result.Items.Select(c => new CedulaEvaluacionDto
                {
                    Id = c.Id,
                    Anio = c.Anio,
                    MesId = c.MesId,
                    ContratoId = c.ContratoId,
                    InmuebleId = c.InmuebleId,
                    Folio = c.Folio,
                    EstatusId = c.EstatusId,
                    RequiereNC = _respuestas.GetDeductivasByCedula(c.Id, respuestas),
                    Calificacion = c.Calificacion,
                    FechaCreacion = c.FechaCreacion,
                    FechaActualizacion = c.FechaActualizacion
                });

                result.Items = cedulas;
            }

            return result != null ? result : new DataCollection<CedulaEvaluacionDto>();
        }
        
        [Route("getCedulasByAnioMes/{anio}/{mes}/{contrato}")]
        [HttpGet]
        public async Task<DataCollection<CedulaEvaluacionDto>> GetCedulaEvaluacionByAnioMes(int anio, int mes, int contrato)
        {
            var result = await _cedula.GetCedulaEvaluacionByAnioMes(anio, mes, contrato);

            if (result != null)
            {
                var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

                var cedulas = result.Items.Select(c => new CedulaEvaluacionDto
                {
                    Id = c.Id,
                    Anio = c.Anio,
                    MesId = c.MesId,
                    InmuebleId = c.InmuebleId,
                    EstatusId = c.EstatusId,
                    RequiereNC = _respuestas.GetDeductivasByCedula(c.Id, respuestas),
                });

                result.Items = cedulas;
            }

            return result;
        }

        [Route("getCedulaById/{cedula}")]
        [HttpGet]
        public async Task<CedulaEvaluacionDto> GetCedulaEvaluacionById(int cedula)
        {
            var cedulaEvaluacion = await _cedula.GetCedulaById(cedula);

            return cedulaEvaluacion != null ? cedulaEvaluacion : new CedulaEvaluacionDto();
        }

        [Route("getTotalPD/{cedula}")]
        [HttpGet]
        public async Task<decimal> GetTotalPDAsync(int cedula)
        {
            return await _respuestas.GetTotalPenasDeductivas(cedula);
        }
    }
}
