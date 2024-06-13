﻿using MediatR;
using Mensajeria.Domain.DOficios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.EventHandler.Commands.Oficios
{
    public class DetalleOficioCreateCommand : IRequest<DetalleOficio>
    {
        public int ServicioId { get; set; }
        public int OficioId { get; set; }
        public int FacturaId { get; set; }
        public int CedulaId { get; set; }
    }
}
