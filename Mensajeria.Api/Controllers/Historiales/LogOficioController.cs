using Mensajeria.Service.EventHandler.Commands.LogCedulas;
using Mensajeria.Service.Queries.DTOs.LogCedulas;
using Mensajeria.Service.Queries.Queries.LogCedulas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mensajeria.Service.Queries.Queries.LogOficios;
using Mensajeria.Service.EventHandler.Commands.LogOficios;
using Mensajeria.Service.Queries.DTOs.LogOficios;

namespace Mensajeria.Api.Controllers.Historiales
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/logOficios")]
    public class LogOficioController : ControllerBase
    {
        private readonly ILogOficioQueryService _logs;
        private readonly IMediator _mediator;

        public LogOficioController(ILogOficioQueryService logs, IMediator mediator)
        {
            _logs = logs;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getHistorialByOficio/{oficio}")]
        public async Task<List<LogOficioDto>> GetHistorialByOficio(int oficio)
        {
            return await _logs.GetHistorialOficio(oficio);
        }

        [HttpPost]
        [Route("createHistorial")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogOficiosCreateCommand historial)
        {
            await _mediator.Send(historial);
            return Ok();
        }
    }
}
