using MediatR;
using Mensajeria.Domain.DOficios;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.EventHandler.Commands.Oficios
{
    public class PagarOficioCommand : IRequest<Oficio>
    {
        public int Id { get; set; }
        public int ESucesivoId { get; set; }
        public int EFacturaId { get; set; }
        public int ENotaCreditoId { get; set; }
        public int ECedulaId { get; set; }
        public string UsuarioId { get; set; }
        public DateTime FechaPago { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Observaciones { get; set; }
    }
}
