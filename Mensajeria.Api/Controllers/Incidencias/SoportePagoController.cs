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
using Newtonsoft.Json;
using Microsoft.AspNetCore.Hosting;

namespace Mensajeria.Api.Controllers.Incidencias
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/soportePago")]
    public class SoportePagoController : ControllerBase
    {
        private readonly ISoportePagoQueryService _soporte;
        private readonly IHostingEnvironment _environment;
        private readonly IMediator _mediator;

        public SoportePagoController(ISoportePagoQueryService soporte, IMediator mediator, IHostingEnvironment environment)
        {
            _soporte = soporte;
            _mediator = mediator;
            _environment = environment;
        }

        [Route("getSoportePagoByCedula/{cedula}")]
        [HttpGet]
        public async Task<List<SoportePagoDto>> GetSoportePagoByCedula(int cedula)
        {
            var soporte = await _soporte.GetSoportePagoByCedula(cedula);

            return soporte;
        }
        
        [Route("getGuiasPendientes/{cedula}")]
        [HttpGet]
        public async Task<List<SoportePagoDto>> GetGuiasPendientes(int cedula)
        {
            var soporte = await _soporte.GetGuiasPendientes(cedula);

            return soporte;
        }
        
        [Route("getSoportePagoByCliente/{cedula}")]
        [HttpGet]
        public async Task<List<SoportePagoDto>> GetSoportePagoByCliente(string cliente)
        {
            var soporte = await _soporte.GetSoportePagoByCliente(cliente);

            return soporte;
        }

        [Route("getSoportePagoById/{soporte}")]
        [HttpGet]
        public async Task<SoportePagoDto> GetSoportePagoById(int soporte)
        {
            var result = await _soporte.GetSoportePagoById(soporte);

            return result;
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("insertaSoportePago")]
        [HttpPost]
        public async Task<IActionResult> InsertaSoportePago([FromForm] SoportePagoCreateCommand incidencia) 
        {
            incidencia.CedulasEvaluacion = JsonConvert.DeserializeObject<List<CedulaSoporteCommand>>(incidencia.Cedulas);
            var incidencias = await _mediator.Send(incidencia);
            return Ok(incidencias);
        }
        
        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("actualizaSoportePago")]
        [HttpPost]
        public async Task<IActionResult> ActualizaSoportePago([FromForm] SoportePagoUpdateCommand incidencia) 
        {
            var incidencias = await _mediator.Send(incidencia);
            return Ok(incidencias);
        }
    }
}
