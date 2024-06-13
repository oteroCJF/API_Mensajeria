using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mensajeria.Service.Queries.DTOs.Cuestionario;
using Mensajeria.Service.Queries.Queries.Cuestionarios;

namespace Mensajeria.Api.Controllers.Cuestionarios.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/cuestionarios")]

    public class CuestionarioController : ControllerBase
    {
        private readonly ICuestionariosQueryService _cuestionario;
        //private readonly IMediator mediator;

        public CuestionarioController(ICuestionariosQueryService cuestionario)
        {
            _cuestionario = cuestionario;
        }

        [HttpGet]
        public async Task<List<CuestionarioDto>> GetAllPreguntas()
        {
            return await _cuestionario.GetAllPreguntasAsync();
        }

        [Route("{anio}/{mes}/{contrato}")]
        [HttpGet]
        public async Task<List<CuestionarioMensualDto>> GetCuestionarioMensualId(int anio, int mes, int contrato)
        {
            return await _cuestionario.GetCuestionarioMensualAsync(anio, mes, contrato);
        }
        
        [Route("getPreguntasConDeductiva/{anio}/{mes}/{contrato}")]
        [HttpGet]
        public async Task<List<CuestionarioMensualDto>> GetPreguntasConDeductiva(int anio, int mes, int contrato)
        {
            return await _cuestionario.GetPreguntasDeductiva(anio, mes, contrato);
        }

        
        [Route("getPreguntaById/{pregunta}")]
        [HttpGet]
        public async Task<CuestionarioDto> GetPreguntaById(int pregunta)
        {
            return await _cuestionario.GetPreguntaByIdAsync(pregunta);
        }
    }
}
