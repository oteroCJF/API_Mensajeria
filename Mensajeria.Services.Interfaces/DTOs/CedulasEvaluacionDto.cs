using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.Queries.DTOs
{
    public class CedulasEvaluacionDto
    {
        public int Id { get; set; }
        public int InmuebleId { get; set; }
        public int UsuarioId { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string Folio { get; set; }
        public string Estatus { get; set; }
        public decimal Calificacion { get; set; }
        public decimal PenaCalificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public IEnumerable<RespuestasDto> respuestas { get; set; } = new List<RespuestasDto>();
    }
}
