using Mensajeria.Service.Queries.DTOs.FlujoMensajeria;
using Mensajeria.Service.Queries.Queries.Flujo;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensajeria.Api.Controllers.Flujo
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/limpieza/flujo")]
    public class FlujoMensajeriaController : ControllerBase
    {
        private readonly IFlujoCedulaQueryService _flujo;

        public FlujoMensajeriaController(IFlujoCedulaQueryService flujo)
        {
            _flujo = flujo;
        }

        [Route("getFlujoByCedulaEstatus/{estatus}")]
        [HttpGet]
        public async Task<List<FlujoCedulaDto>> getFlujoByCedulaEstatus(int estatus)
        {
            var flujo = await _flujo.GetFlujoByEstatus(estatus);

            return flujo;
        }
    }
}
