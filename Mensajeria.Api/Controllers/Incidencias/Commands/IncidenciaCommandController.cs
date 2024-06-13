using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Mensajeria.Service.Queries.DTOs.Incidencias;
using Mensajeria.Service.EventHandler.Commands.Incidencias;
using MediatR;
using Mensajeria.Service.Queries.Queries.Incidencias;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace Mensajeria.Api.Controllers.Incidencias.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/incidenciasCedula")]
    public class IncidenciaCommandController : ControllerBase
    {
        private readonly IIncidenciasQueryService _incidenciasQuery;
        private readonly IMediator _mediator;

        public IncidenciaCommandController(IMediator mediator, IIncidenciasQueryService incidenciasQuery)
        {
            _incidenciasQuery = incidenciasQuery;
            _mediator = mediator;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("insertaIncidencia")]
        [HttpPost]
        public async Task<IActionResult> InsertaIncidencia([FromForm] IncidenciaCreateCommand incidencia)
        {
            await _mediator.Send(incidencia);
            return Ok();
        }

        [Route("actualizarIncidencia")]
        [HttpPut]
        public async Task<IActionResult> ActualizarIncidencia([FromForm] IncidenciaUpdateCommand incidencia)
        {
            await _mediator.Send(incidencia);
            return Ok();
        }

        [Route("eliminarIncidencias")]
        [HttpPost]
        public async Task<IActionResult> EliminarIncidencias([FromBody] IncidenciaDeleteCommand incidencia)
        {
            var lIncidencias = await _incidenciasQuery.GetIncidenciasByPreguntaAndCedula(incidencia.CedulaEvaluacionId, incidencia.Pregunta);
            
            foreach (var inc in lIncidencias) 
            {
                incidencia.Id = inc.Id;
                await _mediator.Send(incidencia);
            }

            return Ok(lIncidencias.Count);
        }

        [Route("eliminarIncidencia")]
        [HttpPost]
        public async Task<IActionResult> EliminarIncidencia([FromBody] IncidenciaDeleteCommand request)
        {
            var incidencia = await _mediator.Send(request);

            return Ok(incidencia);
        }

        [Consumes("multipart/form-data")]
        [Route("insertaIncidenciaExcel")]
        [HttpPost]
        public async Task<IActionResult> InsertaIncidenciaExcel([FromForm] IncidenciaExcelCreateCommand incidencia)
        {
            var incidencias = await _mediator.Send(incidencia);
            return Ok(incidencias);
        }

    }
}
