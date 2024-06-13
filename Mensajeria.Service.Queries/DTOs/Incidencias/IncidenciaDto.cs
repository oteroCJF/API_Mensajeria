using System;

namespace Mensajeria.Service.Queries.DTOs.Incidencias
{
    public class IncidenciaDto
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int IndemnizacionId { get; set; }
        public Nullable<int> EstatusId { get; set; }//verificar si se generará otra tabla debido al modulo de seguimientos
        public int Pregunta { get; set; }
        public string NumeroGuia { get; set; }
        public string CodigoRastreo { get; set; }
        public decimal Sobrepeso { get; set; }
        public string TipoServicio { get; set; }
        public string Acuse { get; set; }
        public int TotalAcuses { get; set; }
        public string? Acta { get; set; }
        public string? Escrito { get; set; }
        public string? Comprobante { get; set; }
        public string? Observaciones { get; set; }
        public bool Penalizable { get; set; }
        public Nullable<decimal> MontoPenalizacion { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
    }
}
