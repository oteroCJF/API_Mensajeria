using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Mensajeria.Service.Queries.DTOs.Facturas;
using Mensajeria.Service.EventHandler.Commands.Repositorios;
using Mensajeria.Service.Queries.Queries.Repositorios;

namespace Mensajeria.Api.Controllers.Repositorios.Commands
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/repositorios")]
    public class RepositorioCommandController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RepositorioCommandController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [Route("createRepositorio")]
        [HttpPost]
        public async Task<IActionResult> CreateRepositorio([FromBody] RepositorioCreateCommand Repositorio)
        {
            int status = await _mediator.Send(Repositorio);
            return Ok(status);
        }

        [Route("updateRepositorio")]
        [HttpPut]
        public async Task<IActionResult> UpdateRepositorio([FromBody] RepositorioUpdateCommand Repositorio)
        {
            var status = await _mediator.Send(Repositorio);
            return Ok(status);
        }
    }
}
