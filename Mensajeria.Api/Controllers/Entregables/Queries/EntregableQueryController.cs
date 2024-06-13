using Mensajeria.Service.EventHandler.Commands.Entregables;
using Mensajeria.Service.Queries.DTOs.Entregables;
using Mensajeria.Service.Queries.Queries.Entregables;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Mensajeria.Service.EventHandler.Commands.LogEntregables;
using System;
using System.Linq;
using Mensajeria.Service.Queries.DTOs.CedulaEvaluacion;
using Mensajeria.Service.Queries.Queries.CedulasEvaluacion;
using System.Globalization;
using Service.Common.Collection;

namespace Mensajeria.Api.Controllers.Entregables.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/entregables")]
    public class EntregableQueryController : ControllerBase
    {
        private readonly IEntregableQueryService _entregables;
        private readonly IHostingEnvironment _environment;

        public EntregableQueryController(IEntregableQueryService entregables, IHostingEnvironment environment)
        {
            _entregables = entregables;
            _environment = environment;
        }

        [HttpGet]
        public async Task<List<EntregableDto>> GetAllEntregables()
        {
            var configuracion = await _entregables.GetAllEntregablesAsync();

            return configuracion;
        }

        [Route("getEntregablesByCedula/{cedula}")]
        [HttpGet]
        public async Task<List<EntregableDto>> GetEntregablesByCedula(int cedula)
        {
            var entregables = await _entregables.GetEntregablesByCedula(cedula);

            return entregables;
        }

        [Route("getEntregableById/{entregable}")]
        [HttpGet]
        public async Task<EntregableDto> GetEntregableById(int entregable)
        {
            var entregables = await _entregables.GetEntregableById(entregable);

            return entregables;
        }

        [Route("getEntregablesValidados")]
        [HttpGet]
        public async Task<DataCollection<EntregableDto>> GetEntregablesValidados()
        {
            var entregables = await _entregables.GetEntregablesValidados();

            return entregables;
        }


        [Route("visualizarEntregable/{anio}/{mes}/{folio}/{archivo}/{tipo}")]
        [HttpGet]
        public async Task<string> VisualizarEntregable(int anio, string mes, string folio, string archivo, string tipo)
        {
            string folderName = Directory.GetCurrentDirectory() + "\\Entregables\\" + anio + "" + "\\" + mes + "\\" + folio + "\\" + tipo;
            string webRootPath = _environment.ContentRootPath;
            string newPath = Path.Combine(webRootPath, folderName);
            string pathArchivo = Path.Combine(newPath, archivo);

            if (System.IO.File.Exists(pathArchivo))
            {
                return pathArchivo;
            }

            return "";
        }

        [Route("getPathEntregables")]
        [HttpGet]
        public async Task<string> GetPathEntregables()
        {
            string folderName = Directory.GetCurrentDirectory() + "\\Entregables";

            return folderName;
        }
    }
}
