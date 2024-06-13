using System;

namespace Mensajeria.Service.Queries.DTOs.Facturas
{
    public class RepositorioDto
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public int MesId { get; set; }
        public int Anio { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int FacturasCargadas { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
