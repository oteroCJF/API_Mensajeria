using MediatR;
using Mensajeria.Domain.DFacturas;
using Microsoft.AspNetCore.Http;

namespace Mensajeria.Service.EventHandler.Commands.Facturas
{
    public class FacturaCreateCommand : IRequest<Factura>
    {
        public int Anio { get; set; }
        public int InmuebleId { get; set; }
        public int RepositorioId { get; set; }
        public string Mes { get; set; }
        public string UsuarioId { get; set; }
        public string Inmueble { get; set; }
        public string TipoFacturacion { get; set; }
        public IFormFile XML { get; set; }
        public IFormFile PDF { get; set; }
    }
}
