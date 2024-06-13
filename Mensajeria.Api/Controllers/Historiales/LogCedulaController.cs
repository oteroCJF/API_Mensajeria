using Mensajeria.Service.EventHandler.Commands.LogCedulas;
using Mensajeria.Service.Queries.DTOs.LogCedulas;
using Mensajeria.Service.Queries.Queries.LogCedulas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensajeria.Api.Controllers.Historiales
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/logCedulas")]
    public class LogCedulaController : ControllerBase
    {
        private readonly ILogCedulasQueryService _logs;
        private readonly IMediator _mediator;

        public LogCedulaController(ILogCedulasQueryService logs, IMediator mediator)
        {
            _logs = logs;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getHistorialByCedula/{cedula}")]
        public async Task<List<LogCedulaDto>> GetHistorialByCedula(int cedula)
        {
            return await _logs.GetHistorialCedula(cedula);
        }

        [HttpPost]
        [Route("createHistorial")]
        public async Task<IActionResult> CreateHistorial([FromBody] LogCedulasCreateCommand historial)
        {
            await _mediator.Publish(historial);
            return Ok();
        }
    }
}
