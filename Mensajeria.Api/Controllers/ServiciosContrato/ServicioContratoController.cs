using Mensajeria.Service.EventHandler.Commands.ServiciosContrato;
using Mensajeria.Service.Queries.DTOs.Contratos;
using Mensajeria.Service.Queries.Queries.ServiciosContrato;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensajeria.Api.Controllers.ServicioContrato
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/servicioContrato")]
    public class ServicioContratoController : ControllerBase
    {
        private readonly IServicioContratoQueryService _scontrato;
        private readonly IMediator _mediator;

        public ServicioContratoController(IServicioContratoQueryService scontrato, IMediator mediator)
        {
            _scontrato = scontrato;
            _mediator = mediator;
        }

        [Route("getServiciosContrato/{contrato}")]
        [HttpGet]
        public async Task<List<ServicioContratoDto>> GetServiciosByContrato(int contrato)
        {
            return await _scontrato.GetServicioContratoListAsync(contrato);
        }

        [Route("createSContrato")]
        [HttpPost]
        public async Task<IActionResult> CreateContrato([FromBody] ServicioContratoCreateCommand request)
        {
            var contrato = await _mediator.Send(request);
            return Ok(contrato);
        }

        [Route("updateSContrato")]
        [HttpPut]
        public async Task<IActionResult> UpdateContrato([FromBody] ServicioContratoUpdateCommand request)
        {
            var contrato = await _mediator.Send(request);
            return Ok(contrato);
        }

        [Route("deleteSContrato")]
        [HttpPut]
        public async Task<IActionResult> DeleteContrato([FromBody] ServicioContratoDeleteCommand request)
        {
            var contrato = await _mediator.Send(request);
            return Ok(contrato);
        }
    }
}
