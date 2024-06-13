using Mensajeria.Service.EventHandler.Commands.Firmantes;
using Mensajeria.Service.Queries.DTOs.Firmantes;
using Mensajeria.Service.Queries.Queries.Firmantes;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensajeria.Api.Controllers.Firmantes
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/firmantes")]
    public class FirmanteController : ControllerBase
    {
        private readonly IFirmantesQueryService _firmantes;
        private readonly IMediator _mediator;

        public FirmanteController(IFirmantesQueryService firmantes, IMediator mediator)
        {
            _firmantes = firmantes;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<List<FirmanteDto>> GetAllFirmantesAsync()
        {
            var firmantes = await _firmantes.GetAllFirmantesAsync();
            return firmantes;
        }
        
        [HttpGet]
        [Route("getFirmantesByInmueble/{inmueble}")]
        public async Task<List<FirmanteDto>> GetFirmantesByInmueble(int inmueble)
        {
            var firmantes = await _firmantes.GetFirmantesByInmueble(inmueble);
            return firmantes;
        }
        
        [HttpGet]
        [Route("getFirmanteById/{firmante}")]
        public async Task<FirmanteDto> GetFirmanteById(int firmante)
        {
            var firmantes = await _firmantes.GetFirmanteById(firmante);
            return firmantes;
        }

        [HttpPost]
        [Route("createFirmantes")]
        public async Task<IActionResult> CreateFirmantes([FromBody] FirmantesCreateCommand firmantes)
        {
            var firmante = await _mediator.Send(firmantes);
            return Ok(firmante);
        }

        [HttpPut]
        [Route("updateFirmantes")]
        public async Task<IActionResult> UpdateFirmantes([FromBody] FirmantesUpdateCommand firmantes)
        {
            var firmante = await _mediator.Send(firmantes);
            return Ok(firmante);
        }
    }
}
