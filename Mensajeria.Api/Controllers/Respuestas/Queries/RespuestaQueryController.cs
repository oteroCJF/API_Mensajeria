using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using MediatR;
using Mensajeria.Service.Queries.Queries.Respuestas;
using Mensajeria.Service.Queries.DTOs.Respuestas;

namespace Mensajeria.Api.Controllers.Respuestas.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/respuestasEvaluacion")]
    public class RespuestaQueryController : ControllerBase
    {
        private readonly IRespuestasQueryService _respuestas;
        private readonly IMediator _mediator;

        public RespuestaQueryController(IRespuestasQueryService respuestas, IMediator mediator)
        {
            _respuestas = respuestas;
            _mediator = mediator;
        }

        [Route("getRespuestasByAnio/{anio}")]
        [HttpGet]
        public async Task<List<RespuestaDto>> GetAllRespuestasByAnioAsync(int anio)
        {
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            return respuestas;
        }

        [Route("getRespuestasByCedula/{cedula}")]
        [HttpGet]
        public async Task<List<RespuestaDto>> GetRespuestasByCedulaAsync(int cedula)
        {
            var respuestas = await _respuestas.GetRespuestasByCedulaAsync(cedula);

            return respuestas;
        }
        
        [Route("getDeductivasByCedula/{cedula}/{anio}")]
        [HttpGet]
        public async Task<bool> GetDeductivasByCedula(int cedula, int anio)
        {
            var respuestas = await _respuestas.GetAllRespuestasByAnioAsync(anio);

            var requiereNC = _respuestas.GetDeductivasByCedula(cedula,respuestas);

            return requiereNC;
        }
    }
}
