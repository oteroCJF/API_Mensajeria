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

namespace Mensajeria.Api.Controllers.Incidencias.Queries
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/mensajeria/incidenciasCedula")]
    public class IncidenciaQueryController : ControllerBase
    {
        private readonly IIncidenciasQueryService _incidencias;
        private readonly IHostingEnvironment _environment;

        public IncidenciaQueryController(IIncidenciasQueryService incidencias, IHostingEnvironment environment)
        {
            _incidencias = incidencias;
            _environment = environment;
        }

        [Route("getConfiguracionIncidencias")]
        [HttpGet]
        public async Task<List<ConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            var configuracion = await _incidencias.GetConfiguracionIncidencias();

            return configuracion;
        }

        [Route("getConfiguracionIncidenciasByPregunta/{pregunta}/{respuesta}")]
        [HttpGet]
        public async Task<ConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            var configuracion = await _incidencias.GetConfiguracionIncidenciasByPregunta(pregunta, respuesta);

            return configuracion;
        }

        [Route("getIncidenciasByCedula/{cedula}")]
        [HttpGet]
        public async Task<List<IncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            var incidencias = await _incidencias.GetIncidenciasByCedula(cedula);

            return incidencias;
        }
        
        [Route("getIncidenciasByCedulaAndPregunta/{cedula}/{pregunta}")]
        [HttpGet]
        public async Task<List<IncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciasByPreguntaAndCedula(cedula, pregunta);

            return incidencias;
        }

        [Route("getIncidenciaByCedulaAndPregunta/{cedula}/{pregunta}")]
        [HttpGet]
        public async Task<IncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            var incidencias = await _incidencias.GetIncidenciaByPreguntaAndCedula(cedula, pregunta);

            return incidencias != null ? incidencias : new IncidenciaDto();
        }


        [Route("visualizarActas/{anio}/{mes}/{folio}/{tipo}/{tipoArchivo}/{archivo}")]
        [HttpGet]
        public string VisualizarActas(int anio, string mes, string folio, string tipo, string tipoArchivo, string archivo)
        {
            string folderName = Directory.GetCurrentDirectory() + "\\Entregables\\" + anio + "" + "\\" + mes + "\\" + folio + "\\Actas " + tipo.Replace("Guías ","") + "\\"+tipoArchivo;
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
