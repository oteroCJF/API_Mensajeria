using Mensajeria.Service.EventHandler.Commands.Entregables;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Mensajeria.Service.EventHandler.Commands.LogEntregables;
using System;
using Mensajeria.Service.EventHandler.Commands.Entregables.Update;
using Mensajeria.Service.EventHandler.Commands.Entregables.Create;
using Mensajeria.Service.EventHandler.Commands.Entregables.Delete;

namespace Mensajeria.Api.Controllers.Entregables.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/entregables")]
    public class EntregableCommandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public EntregableCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("createEntregable")]
        [HttpPost]
        public async Task<IActionResult> CreateEntregable([FromBody] EntregableCreateCommand request)
        {
            var entregable = await _mediator.Send(request);

            if (entregable != null)
            {
                var log = new LogEntregablesCreateCommand
                {
                    CedulaEvaluacionId = entregable.CedulaEvaluacionId,
                    EntregableId = entregable.EntregableId,
                    UsuarioId = request.UsuarioId,
                    EstatusId = entregable.EstatusId,
                    Observaciones = "Se liberó el entregable "+request.Entregable,
                    FechaCreacion = DateTime.Now
                };

                await _mediator.Send(log);
            }
            return Ok(entregable);
        }

        [Consumes("multipart/form-data")]
        [Route("actualizarEntregable")]
        [HttpPut]
        public async Task<IActionResult> UpdateEntregable([FromForm] EntregableUpdateCommand request)
        {
            var entregable = await _mediator.Send(request);

            if (entregable != null)
            {
                var log = new LogEntregablesCreateCommand
                {
                    CedulaEvaluacionId = entregable.CedulaEvaluacionId,
                    EntregableId = entregable.EntregableId,
                    UsuarioId = request.UsuarioId,
                    EstatusId = entregable.EstatusId,
                    Observaciones = request.Observaciones,
                    FechaCreacion = DateTime.Now
                };

                await _mediator.Send(log);
            }
            return Ok();
        }

        [Route("deleteEntregable")]
        [HttpPut]
        public async Task<IActionResult> DeleteEntregable([FromBody] EntregableDeleteCommand request)
        {
            var entregable = await _mediator.Send(request);

            if (entregable != null)
            {
                var log = new LogEntregablesCreateCommand
                {
                    CedulaEvaluacionId = entregable.CedulaEvaluacionId,
                    EntregableId = entregable.EntregableId,
                    UsuarioId = request.UsuarioId,
                    EstatusId = entregable.EstatusId,
                    Observaciones = "Se eliminó el entregable " + request.TipoEntregable,
                    FechaCreacion = DateTime.Now
                };

                await _mediator.Send(log);
            }
            return Ok(entregable);
        }

        [Route("updateEntregableSR")]
        [HttpPut]
        public async Task<IActionResult> UpdateEntregableSR([FromBody] ESREntregableUpdateCommand request)
        {
            var entregable = await _mediator.Send(request);
            if (entregable != null)
            {
                var log = new LogEntregablesCreateCommand
                {
                    CedulaEvaluacionId = entregable.CedulaEvaluacionId,
                    EntregableId = entregable.EntregableId,
                    UsuarioId = entregable.UsuarioId,
                    EstatusId = !request.Aprobada ? entregable.EstatusId : request.EstatusId,
                    Observaciones = request.Observaciones,
                    FechaCreacion = DateTime.Now
                };

                await _mediator.Send(log);
            }
            return Ok();
        }

        [Route("autorizarEntregable")]
        [HttpPut]
        public async Task<IActionResult> AUpdateEntregable([FromBody] EEntregableUpdateCommand request)
        {
            var entregable = await _mediator.Send(request);
            if (entregable != null)
            {
                var log = new LogEntregablesCreateCommand
                {
                    CedulaEvaluacionId = entregable.CedulaEvaluacionId,
                    EntregableId = entregable.EntregableId,
                    UsuarioId = request.UsuarioId,
                    EstatusId = entregable.EstatusId,
                    Observaciones = request.Observaciones,
                    FechaCreacion = DateTime.Now
                };

                await _mediator.Send(log);
            }
            return Ok();
        }
    }
}
