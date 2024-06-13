using System;

namespace Mensajeria.Service.Queries.DTOs.Respuestas
{
    public class RespuestaDto
    {
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public System.Nullable<bool> Respuesta { get; set; }
        public string? Detalles { get; set; } 
        public System.Nullable<bool> Penalizable { get; set; }
        public System.Nullable<decimal> MontoPenalizacion { get; set; } = 0;
        public System.Nullable<DateTime> FechaCreacion { get; set; } 
        public System.Nullable<DateTime> FechaActualizacion { get; set; } 
        public System.Nullable<DateTime> FechaEliminacion { get; set; } 
    }
}
