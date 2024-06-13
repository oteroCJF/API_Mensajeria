using System;
using System.Collections.Generic;
using System.Text;

namespace Limpieza.Service.Queries.DTOs
{
    public class RespuestasDto
    {
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public int PreguntaReal { get; set; }
        public bool Respuesta { get; set; }
        public string Detalles { get; set; }
        public bool? Penalizable { get; set; }
        public decimal MontoPenalizacion { get; set; }
    }
}
