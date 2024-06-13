using Mensajeria.Service.EventHandler.Commands.Contratos;
using Mensajeria.Service.EventHandler.Commands.Convenios;
using Mensajeria.Service.Queries.DTOs.Contratos;
using Mensajeria.Service.Queries.Queries.Convenios;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensajeria.Api.Controllers.Convenio
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/convenios")]
    public class ConvenioController : ControllerBase
    {
        private readonly IConvenioQueryService _convenio;
        private readonly IMediator _mediator;

        public ConvenioController(IConvenioQueryService convenio, IMediator mediator)
        {
            _convenio = convenio;
            _mediator = mediator;
        }

        [HttpGet]
        [Route("getConveniosByContrato/{contrato}")]
        public async Task<List<ConvenioDto>> getConveniosByContrato(int contrato)
        {
            List<ConvenioDto> convenios = await _convenio.GetConveniosByContratoAsync(contrato);
            return convenios;
        }

        [HttpGet]
        [Route("getConvenioById/{convenio}")]
        public async Task<ConvenioDto> getConveniosById(int convenio)
        {
            ConvenioDto convenios = await _convenio.GetConvenioByIdAsync(convenio);
            return convenios;
        }

        [Route("createConvenio")]
        [HttpPost]
        public async Task<IActionResult> CreateConvenio([FromBody] ConvenioCreateCommand contrato)
        {
            int success = await _mediator.Send(contrato);
            return Ok(success);
        }

        [Route("updateConvenio")]
        [HttpPut]
        public async Task<IActionResult> UpdateConvenio([FromBody] ConvenioUpdateCommand contrato)
        {
            int success = await _mediator.Send(contrato);
            return Ok(success);
        }

        [Route("deleteConvenio")]
        [HttpPut]
        public async Task<IActionResult> DeleteConvenio([FromBody] ConvenioDeleteCommand contrato)
        {
            int success = await _mediator.Send(contrato);
            return Ok(success);
        }

        [HttpGet]
        [Route("getRubrosByConvenio/{convenio}")]
        public async Task<List<RubroConvenioDto>> GetRubrosConvenio(int convenio)
        {
            List<RubroConvenioDto> convenios = await _convenio.GetRubrosConvenioAsync(convenio);
            return convenios;
        }
    }
}
