using Mensajeria.Service.Queries.DTOs.CedulaEvaluacion;
using Mensajeria.Service.Queries.Queries.Incidencias;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mensajeria.Service.Queries.Queries.CedulasEvaluacion;
using Mensajeria.Service.EventHandler.Commands.CedulasEvaluacion;
using Mensajeria.Service.EventHandler.Commands.LogCedulas;
using Mensajeria.Service.EventHandler.Commands.CedulasEvaluacion.DBloquearCedula;
using Mensajeria.Service.EventHandler.Commands.CedulasEvaluacion.ActualizacionCedula;
using Mensajeria.Service.Queries.Queries.Respuestas;

namespace Mensajeria.Api.Controllers.CedulasEvaluacion.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/cedulaEvaluacion")]
    public class MensajeriaCommandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public MensajeriaCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("enviarCedula")]
        [HttpPut]
        public async Task<IActionResult> EnviarCedula([FromBody] EnviarCedulaEvaluacionUpdateCommand request)
        {
            var cedula = await _mediator.Send(request);

            if (cedula != null)
            {
                var log = new LogCedulasCreateCommand
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = cedula.Id,
                    EstatusId = request.EstatusId,
                    Observaciones = request.Observaciones
                };

                var logs = await _mediator.Send(log);

                if (logs != null)
                {
                    return Ok(cedula);
                }
            }
            return Ok(cedula);
        }

        [Route("dbloquearCedula")]
        [HttpPut]
        public async Task<IActionResult> DBloquearCedula([FromBody] DBloquearCedulaUpdateCommand request)
        {
            var cedula = await _mediator.Send(request);

            if (cedula != null)
            {
                var log = new LogCedulasCreateCommand
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = cedula.Id,
                    EstatusId = cedula.EstatusId,
                    Observaciones = request.Observaciones
                };

                var logs = await _mediator.Send(log);

                if (logs != null)
                {
                    return Ok(cedula);
                }
            }
            return Ok(cedula);
        }

        [Route("updateCedula")]
        [HttpPut]
        public async Task<IActionResult> UpdateCedula([FromBody] CedulaEvaluacionUpdateCommand request)
        {
            var cedula = await _mediator.Send(request);

            if (cedula != null)
            {
                var log = new LogCedulasCreateCommand
                {
                    UsuarioId = request.UsuarioId,
                    CedulaEvaluacionId = cedula.Id,
                    EstatusId = cedula.EstatusId,
                    Observaciones = request.Observaciones
                };

                var logs = await _mediator.Send(log);

                if (logs != null)
                {
                    return Ok(cedula);
                }
            }
            return Ok(cedula);
        }
    }
}
