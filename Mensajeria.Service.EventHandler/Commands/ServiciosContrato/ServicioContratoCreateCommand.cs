using MediatR;
using Mensajeria.Domain.DContratos;

namespace Mensajeria.Service.EventHandler.Commands.ServiciosContrato
{
    public class ServicioContratoCreateCommand : IRequest<ServicioContrato>
    {
        public int ContratoId { get; set; }
        public int ServicioId { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal IVA { get; set; }
        public decimal PorcentajeImpuesto { get; set; }
    }
}
