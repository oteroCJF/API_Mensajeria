using Mensajeria.Domain.DFacturas;
using Mensajeria.Persistence.Database;
using Mensajeria.Service.Queries.DTOs.Facturas;
using Mensajeria.Service.Queries.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mensajeria.Service.Queries.Queries.Facturas
{
    public interface IFacturasQueryService
    {
        Task<List<FacturaDto>> GetAllFacturas();
        Task<List<FacturaDto>> GetAllFacturasAsync(int Repositorio);
        Task<List<FacturaDto>> GetFacturasByInmuebleAsync(int inmueble, int Repositorio);
        Task<List<ConceptoFacturaDto>> GetConceptosFacturaByIdAsync(int factura);
        Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int Repositorio);
        Task<List<FacturaDto>> GetFacturasNCPendientes(int estatus);
        Task<int> GetFacturasCargadasAsync(int Repositorio);
        Task<int> GetNotasCreditoCargadasAsync(int Repositorio);
        Task<int> GetTotalFacturasByInmuebleAsync(int Repositorio, int inmueble);
        Task<int> GetNCByInmuebleAsync(int Repositorio, int inmueble);
        Task<decimal> GetFacturacionByCedula(int cedula);
    }

    public class FacturaQueryService : IFacturasQueryService
    {
        private readonly ApplicationDbContext _context;

        public FacturaQueryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<FacturaDto>> GetAllFacturas()
        {
            try
            {
                var facturas = await _context.Facturas.Where(f => !f.FechaEliminacion.HasValue).ToListAsync();

                return facturas.MapTo<List<FacturaDto>>();
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<FacturaDto>> GetAllFacturasAsync(int Repositorio)
        {
            try
            {
                var facturas = await _context.Facturas.Where(f => f.RepositorioId == Repositorio && !f.FechaEliminacion.HasValue).ToListAsync();

                return facturas.MapTo<List<FacturaDto>>();
            } 
            catch (Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<FacturaDto>> GetFacturasByInmuebleAsync(int inmueble, int Repositorio)
        {
            var collection = await _context.Facturas.Where(x => x.InmuebleId == inmueble && x.RepositorioId == Repositorio && !x.FechaEliminacion.HasValue).OrderBy(x => x.InmuebleId).ToListAsync();

            return collection.MapTo<List<FacturaDto>>();
        }

        public async Task<List<ConceptoFacturaDto>> GetConceptosFacturaByIdAsync(int factura)
        {
            var conceptos = await _context.ConceptosFactura.Where(x => x.FacturaId == factura)
                                            .GroupBy(c => new {
                                                c.FacturaId,
                                                c.ClaveProducto,
                                                c.ClaveUnidad,
                                                c.Unidad,
                                                c.Descripcion,
                                                c.PrecioUnitario
                                            })
                                            .Select(cf => new ConceptoFacturaDto {
                                                FacturaId = cf.Key.FacturaId,
                                                Cantidad = cf.Sum(c => c.Cantidad),
                                                ClaveProducto = cf.Key.ClaveProducto,
                                                ClaveUnidad = cf.Key.ClaveUnidad,
                                                Unidad = cf.Key.Unidad,
                                                Descripcion = cf.Key.Descripcion,
                                                PrecioUnitario = cf.Key.PrecioUnitario,
                                                Subtotal = cf.Sum(sb => sb.Subtotal),
                                                Descuento = cf.Sum(sb => sb.Descuento),
                                                IVA= (cf.Sum(sb => sb.Subtotal)*Convert.ToDecimal(0.16))
                                            })
                                            .ToListAsync();

            return conceptos.MapTo<List<ConceptoFacturaDto>>();
        }

        public async Task<List<HistorialMFDto>> GetHistorialMFByRepositorio(int Repositorio)
        {
            var historial = await _context.HistorialMF.Where(hf => hf.RepositorioId == Repositorio).OrderBy(i => i.InmuebleId).ToListAsync();
            return historial.MapTo<List<HistorialMFDto>>();
        }

        public async Task<List<FacturaDto>> GetFacturasNCPendientes(int estatus)
        {
            var collection = await _context.Facturas.Where(f => f.EstatusId == estatus && !f.FechaEliminacion.HasValue)
                             .OrderBy(x => x.InmuebleId).ToListAsync();

            return collection.MapTo<List<FacturaDto>>();
        }

        public async Task<int> GetFacturasCargadasAsync(int Repositorio)
        {
            var collection = await _context.Facturas.Where(x => x.RepositorioId == Repositorio && x.Tipo.Equals("Factura") && !x.FechaEliminacion.HasValue)
                            .OrderBy(x => x.InmuebleId).ToListAsync();

            int facturas = collection.Count(x => x.Total > 0);

            return facturas;
        }

        public async Task<int> GetNotasCreditoCargadasAsync(int Repositorio)
        {
            var collection = await _context.Facturas.Where(x => x.RepositorioId == Repositorio && x.Tipo.Equals("NC") && !x.FechaEliminacion.HasValue)
                                .OrderBy(x => x.InmuebleId).ToListAsync();

            int facturas = collection.Count(x => x.Total > 0);

            return facturas;
        }

        public async Task<int> GetTotalFacturasByInmuebleAsync(int Repositorio, int inmueble)
        {
            var collection = await _context.Facturas
                .Where(x => x.RepositorioId == Repositorio && x.InmuebleId == inmueble && x.Tipo.Equals("Factura")).OrderBy(x => x.InmuebleId).ToListAsync();

            int facturas = collection.Count(x => x.Total > 0);

            return facturas;
        }

        public async Task<int> GetNCByInmuebleAsync(int Repositorio, int inmueble)
        {
            var collection = await _context.Facturas.Where(x => x.RepositorioId == Repositorio && x.InmuebleId == inmueble && x.Tipo.Equals("NC"))
                .OrderBy(x => x.InmuebleId).ToListAsync();

            int facturas = collection.Count(x => x.Total > 0);

            return facturas;
        }

        public async Task<decimal> GetFacturacionByCedula(int cedula)
        {
            var facturasId = await _context.Entregables.Where(e => e.CedulaEvaluacionId == cedula &&
                                                                   e.FacturaId != 0 &&
                                                                   e.FacturaId != null && !e.FechaEliminacion.HasValue).Select(e => e.FacturaId).ToListAsync();

            return _context.Facturas.Where(f => facturasId.Contains(f.Id)).Sum(f => f.Total);

        }

    }
}
