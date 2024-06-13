using Mensajeria.Service.EventHandler.Commands.Facturas;
using Mensajeria.Service.Queries.DTOs.Facturas;
using Mensajeria.Service.Queries.Queries.Facturas;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Mensajeria.Service.EventHandler.Commands.CFDIs;
using System.IO;

namespace Mensajeria.Api.Controllers.Facturas.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/cfdi")]
    public class FacturaQueryController : ControllerBase
    {
        private readonly IFacturasQueryService _facturas;
        private readonly IHostingEnvironment _environment;
        private readonly IMediator _mediator;

        public FacturaQueryController(IFacturasQueryService facturas, IMediator mediator, IHostingEnvironment environment)
        {
            _facturas = facturas;
            _mediator = mediator;
            _environment = environment;
        }

        [Route("getAllFacturas")]
        [HttpGet]
        public async Task<List<FacturaDto>> GetAllFacturas()
        {
            var facturas = await _facturas.GetAllFacturas();

            return facturas;
        }

        [Route("getFacturasByRepositorio/{Repositorio}")]
        [HttpGet]
        public async Task<List<FacturaDto>> GetAllFacturas(int Repositorio)
        {
            var facturas = await _facturas.GetAllFacturasAsync(Repositorio);

            return facturas;
        }

        [Route("getFacturasByInmueble/{inmueble}/{Repositorio}")]
        [HttpGet]
        public async Task<List<FacturaDto>> GetFacturasByInmueble(int inmueble, int Repositorio)
        {
            var facturas = await _facturas.GetFacturasByInmuebleAsync(inmueble, Repositorio);

            return facturas;
        }

        [Route("getConceptosFacturaByIdAsync/{factura}")]
        [HttpGet]
        public async Task<List<ConceptoFacturaDto>> GetConceptosFacturaByIdAsync(int factura)
        {
            return await _facturas.GetConceptosFacturaByIdAsync(factura);
        }

        [Route("getFacturasNCPendientes/{estatus}")]
        [HttpGet]
        public async Task<List<FacturaDto>> getFacturasNCPendientes(int estatus)
        {
            return await _facturas.GetFacturasNCPendientes(estatus);
        }

        [Route("getFacturasCargadas/{Repositorio}")]
        [HttpGet]
        public async Task<int> GetFacturasCargadas(int Repositorio)
        {
            return await _facturas.GetFacturasCargadasAsync(Repositorio);
        }

        [Route("getNotasCreditoCargadas/{Repositorio}")]
        [HttpGet]
        public async Task<int> GetNotasCreditoCargadas(int Repositorio)
        {
            return await _facturas.GetNotasCreditoCargadasAsync(Repositorio);
        }

        [Route("getTotalFacturasByInmueble/{inmueble}/{Repositorio}")]
        [HttpGet]
        public async Task<int> GetTotalFacturasByInmueble(int inmueble, int Repositorio)
        {
            return await _facturas.GetTotalFacturasByInmuebleAsync(Repositorio, inmueble);
        }

        [Route("getNCByInmueble/{inmueble}/{Repositorio}")]
        [HttpGet]
        public async Task<int> GetNCByInmueble(int inmueble, int Repositorio)
        {
            return await _facturas.GetNCByInmuebleAsync(Repositorio, inmueble);
        }

        [Route("getFacturacionByCedula/{cedula}")]
        [HttpGet]
        public async Task<decimal> GetFacturacionByCedula(int cedula)
        {
            return await _facturas.GetFacturacionByCedula(cedula);
        }

        [Route("getHistorialMFByRepositorio/{Repositorio}")]
        [HttpGet]
        public async Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int Repositorio)
        {
            var historial = await _facturas.GetHistorialMFByRepositorio(Repositorio);

            HistorialMFDeleteCommand delete = new HistorialMFDeleteCommand();

            delete.RepositorioId = Repositorio;

            await _mediator.Send(delete);

            return historial;
        }

        [Route("visualizarFactura/{anio}/{mes}/{folio}/{tipo}/{inmueble}/{archivo}")]
        [HttpGet]
        public string VisualizarFactura(int anio, string mes, string folio, string tipo, string inmueble, string archivo)
        {
            string folderName = "";
            if (tipo.Equals("NC"))
            {
                folderName = Directory.GetCurrentDirectory() + "\\Repositorio\\" + anio + "\\" + mes + "\\" + inmueble + "\\Notas de Crédito\\" + folio;
            }
            else
            {
                folderName = Directory.GetCurrentDirectory() + "\\Repositorio\\" + anio + "\\" + mes + "\\" + inmueble + "\\Facturas\\" + folio;
            }
            string webRootPath = _environment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, archivo);

            if (System.IO.File.Exists(pathArchivo))
            {
                return pathArchivo;
            }

            return "";
        }
    }
}
