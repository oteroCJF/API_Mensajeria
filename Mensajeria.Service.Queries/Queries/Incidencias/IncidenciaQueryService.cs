﻿using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Incidencias;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Incidencias
{
    public interface IIncidenciasQueryService
    {
        Task<List<IncidenciaDto>> GetAllIncidenciasAsync();
        Task<List<IncidenciaDto>> GetIncidenciasByCedula(int cedula);
        Task<List<IncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta);
        Task<IncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta);
        Task<List<ConfiguracionIncidenciaDto>> GetConfiguracionIncidencias();
        Task<ConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta);
    }

    public class IncidenciaQueryService : IIncidenciasQueryService
    {
        private readonly ApplicationDbContext _context;

        public IncidenciaQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<IncidenciaDto>> GetAllIncidenciasAsync()
        {
            try
            {
                var collection = await _context.Incidencias.Where(x => !x.FechaEliminacion.HasValue).OrderByDescending(x => x.Id).ToListAsync();

                return collection.MapTo<List<IncidenciaDto>>();
            }
            catch(Exception ex) 
            { 
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<IncidenciaDto>> GetIncidenciasByCedula(int cedula)
        {
            try
            {
                var incidencias = await _context.Incidencias
                    .Where(x => x.CedulaEvaluacionId == cedula && !x.FechaEliminacion.HasValue).ToListAsync();

                return incidencias.MapTo<List<IncidenciaDto>>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        
        public async Task<List<IncidenciaDto>> GetIncidenciasByPreguntaAndCedula(int cedula, int pregunta)
        {
            try
            {
                var incidencias = await _context.Incidencias
                    .Where(x => x.CedulaEvaluacionId == cedula && x.Pregunta == pregunta
                    && !x.FechaEliminacion.HasValue).ToListAsync();

                return incidencias.MapTo<List<IncidenciaDto>>();
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<IncidenciaDto> GetIncidenciaByPreguntaAndCedula(int cedula, int pregunta)
        {
            try
            {
                var incidencia = await _context.Incidencias
                    .SingleOrDefaultAsync(x => x.CedulaEvaluacionId == cedula && x.Pregunta == pregunta 
                    && !x.FechaEliminacion.HasValue);

                return incidencia.MapTo<IncidenciaDto>();
            }
            catch (Exception ex)
            { 
                string msg = ex.Message; 
                return null;
            }
        }

        public async Task<List<ConfiguracionIncidenciaDto>> GetConfiguracionIncidencias()
        {
            try
            {
                var incidencia = await _context.ConfiguracionIncidencias.ToListAsync();

                return incidencia.MapTo<List<ConfiguracionIncidenciaDto>>();
            }
            catch(Exception ex) 
            { 
                string message = ex.Message;
                return null;
            }
        }

        public async Task<ConfiguracionIncidenciaDto> GetConfiguracionIncidenciasByPregunta(int pregunta, bool respuesta)
        {
            try
            {
                var incidencia = await _context.ConfiguracionIncidencias.SingleAsync(cn => cn.Pregunta == pregunta && cn.Respuesta == respuesta);

                return incidencia.MapTo<ConfiguracionIncidenciaDto>();
            }
            catch (Exception ex)
            {
                string message = ex.Message;
                return null;
            }
        }

    }
}
