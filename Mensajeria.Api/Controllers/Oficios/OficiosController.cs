using MediatR;
using Mensajeria.Service.EventHandler.Commands.LogOficios;
using Mensajeria.Service.EventHandler.Commands.Oficios;
using Mensajeria.Service.Queries.Queries.Oficios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mensajeria.Api.Controllers.Oficios
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/oficios")]
    public class OficiosController : ControllerBase
    {
        private readonly IOficioQueryService _oficios;
        private readonly IMediator _mediator;

        public OficiosController(IOficioQueryService oficios, IMediator mediator)
        {
            _oficios = oficios;
            _mediator = mediator;
        }

        public async Task<IActionResult> GetAllOficiosAsync() 
        {
            var oficios = await _oficios.GetAllOficiosAsync();
            return Ok(oficios);
        }

        [HttpGet]
        [Route("getOficiosByAnio/{anio}")]
        public async Task<IActionResult> GetOficiosByAnio(int anio)
        {
            var oficios = await _oficios.GetOficiosByAnio(anio);
            return Ok(oficios);
        }
        
        [HttpGet]
        [Route("getOficioById/{id}")]
        public async Task<IActionResult> GetOficioById(int id)
        {
            var oficio = await _oficios.GetOficioById(id);
            return Ok(oficio);
        }
        
        [HttpGet]
        [Route("getDetalleOficio/{id}")]
        public async Task<IActionResult> GetDetalleOficio(int id)
        {
            var oficio = await _oficios.GetDetalleOficio(id);
            return Ok(oficio);
        }

        [Produces("application/json")]
        [Consumes("multipart/form-data")]
        [Route("createOficio")]
        [HttpPost]
        public async Task<IActionResult> CreateOficio([FromForm] OficioCreateCommand request)
        {
            var oficio = await _mediator.Send(request);
            return Ok(oficio);
        }
        
        [Route("createDetalleOficio")]
        [HttpPost]
        public async Task<IActionResult> CreateDetalleOficio([FromBody] List<DetalleOficioCreateCommand> request)
        {
            try
            {
                foreach (var dt in request)
                {
                    await _mediator.Send(dt);
                }

                return Ok(request);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }
        
        [Route("deleteDetalleOficio")]
        [HttpPost]
        public async Task<IActionResult> DeleteDetalleOficio([FromBody] DetalleOficioDeleteCommand request)
        {
            var oficio = await _mediator.Send(request);
            return Ok(oficio);
        }
        
        [Route("cancelarOficio")]
        [HttpPost]
        public async Task<IActionResult> CancelarOficio([FromBody] CancelarOficioCommand request)
        {
            try
            {
                var oficio = await _mediator.Send(request);
                if (oficio != null)
                {
                    LogOficiosCreateCommand logOficio = new LogOficiosCreateCommand
                    {
                        OficioId = request.Id,
                        UsuarioId = request.UsuarioId,
                        EstatusId = request.ESucesivoId,
                        Observaciones = request.Observaciones
                    };

                    var log = await _mediator.Send(logOficio);
                }
                return Ok(oficio);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest();
            }
        }
        
        [Route("corregirOficio")]
        [HttpPost]
        public async Task<IActionResult> CorregirOficio([FromBody] CorregirOficioCommand request)
        {
            try
            {
                var oficio = await _mediator.Send(request);
                if (oficio != null)
                {
                    LogOficiosCreateCommand logOficio = new LogOficiosCreateCommand
                    {
                        OficioId = request.Id,
                        UsuarioId = request.UsuarioId,
                        EstatusId = request.ESucesivoId,
                        Observaciones = request.Observaciones
                    };

                    var log = await _mediator.Send(logOficio);
                }
                return Ok(oficio);
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return BadRequest();
            }
        }
        
        [Route("enviarDGPPTOficio")]
        [HttpPost]
        public async Task<IActionResult> EnviarDGPPTOficio([FromBody] EDGPPTOficioCommand request)
        {
            try
            {
                var oficio = await _mediator.Send(request);
                if (oficio != null)
                {
                    LogOficiosCreateCommand logOficio = new LogOficiosCreateCommand
                    {
                        OficioId = request.Id,
                        UsuarioId = request.UsuarioId,
                        EstatusId = request.ESucesivoId,
                        Observaciones = request.Observaciones
                    };

                    var log = await _mediator.Send(logOficio);
                }
                return Ok(oficio);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest();
            }
        }
        
        [Route("pagarOficio")]
        [HttpPost]
        public async Task<IActionResult> PagarOficio([FromBody] PagarOficioCommand request)
        {
            try
            {
                var oficio = await _mediator.Send(request);
                if (oficio != null)
                {
                    LogOficiosCreateCommand logOficio = new LogOficiosCreateCommand
                    {
                        OficioId = request.Id,
                        UsuarioId = request.UsuarioId,
                        EstatusId = request.ESucesivoId,
                        Observaciones = request.Observaciones
                    };

                    var log = await _mediator.Send(logOficio);
                }
                return Ok(oficio);
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return BadRequest();
            }
        }
    }
}
