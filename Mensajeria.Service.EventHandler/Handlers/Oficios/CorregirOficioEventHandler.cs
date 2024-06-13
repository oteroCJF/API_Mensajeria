﻿using MediatR;
using Mensajeria.Domain.DCedulaEvaluacion;
using Mensajeria.Domain.DFacturas;
using Mensajeria.Domain.DHistorial;
using Mensajeria.Domain.DOficios;
using Mensajeria.Persistence.Database;
using Mensajeria.Service.EventHandler.Commands.Oficios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Mensajeria.Service.EventHandler.Handlers.Oficios
{
    public class CorregirOficioEventHandler : IRequestHandler<CorregirOficioCommand, Oficio>
    {
        private readonly ApplicationDbContext _context;

        public CorregirOficioEventHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Oficio> Handle(CorregirOficioCommand request, CancellationToken cancellationToken)
        {
            try
            {
                request.FechaCreacion = DateTime.Now;
                var oficio = _context.Oficios.Single(o => o.Id == request.Id);
                List<Factura> facturas = await EFacturasTramitePago(request);
                List<CedulaEvaluacion> cedulas = await ECedulasTramitePago(request);

                //var dtOficio = await EliminaDetalleOficio(request.Id);

                if (cedulas != null && facturas != null)
                {
                    oficio.EstatusId = request.ESucesivoId;
                    await _context.SaveChangesAsync();
                }
                
                return oficio;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }

        public async Task<List<Factura>> EFacturasTramitePago(CorregirOficioCommand request)
        { 
            try
            {
                var facturasId = _context.DetalleOficios.Where(dt => dt.OficioId == request.Id).Select(dt => dt.FacturaId).ToList();
                var facturas = _context.Facturas.Where(f => facturasId.Contains(f.Id)).ToList();

                foreach(var fc in facturas)
                {
                    fc.EstatusId = request.EFacturaId;
                    fc.FechaActualizacion = request.FechaCreacion;

                    await _context.SaveChangesAsync();
                }

                return facturas;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        
        public async Task<List<CedulaEvaluacion>> ECedulasTramitePago(CorregirOficioCommand request)
        { 
            try
            {
                var Oficio = _context.Oficios.Single(o => o.Id == request.Id);
                var cedulasId = _context.DetalleOficios.Where(dt => dt.OficioId == request.Id).Select(dt => dt.CedulaId).ToList();
                var cedulas = _context.CedulaEvaluacion.Where(c => cedulasId.Contains(c.Id)).ToList();
                DateTime fechaCreacion = DateTime.Now;

                foreach(var c in cedulas)
                {
                    c.EstatusId = request.ESucesivoId;
                    await _context.SaveChangesAsync();

                    LogCedula logCedula = new LogCedula
                    {
                        CedulaEvaluacionId = c.Id,
                        UsuarioId = request.UsuarioId,
                        EstatusId = request.ESucesivoId,
                        Observaciones = "Se regresa la cédula a <b>Trámite de Pago</b> debido a una corrección del oficio de pago con número <b>"+Oficio.NumeroOficio+"</b]>.",
                        FechaCreacion = request.FechaCreacion
                    };

                    await _context.AddAsync(logCedula);
                    await _context.SaveChangesAsync();
                }

                return cedulas;
            }
            catch(Exception ex)
            {
                string msg = ex.Message;
                return null;
            }
        }
        
        //public async Task<List<DetalleOficio>> EliminaDetalleOficio(int oficio)
        //{ 
        //    try
        //    {
        //        var dtOficio = await _context.DetalleOficios.Where(dt => dt.OficioId == oficio).ToListAsync();

        //        _context.DetalleOficios.RemoveRange(dtOficio);

        //        return dtOficio;
        //    }
        //    catch(Exception ex)
        //    {
        //        string msg = ex.Message;
        //        return null;
        //    }
        //}
    }
}
