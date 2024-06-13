using System;

namespace Mensajeria.Service.Queries.DTOs.Cuestionario
{
    public class CuestionarioDto
    {
        public int Id { get; set; }
        public int NoPregunta { get; set; }
        public string Abreviacion { get; set; }
        public string Concepto { get; set; }
        public string Pregunta { get; set; }
        public string Ayuda { get; set; }
        public string TipoDeduccion { get; set; }
        public string Botones { get; set; }
        public string Icono { get; set; }
        public bool NoAplica { get; set; }
        public bool Incidencias { get; set; }
        public bool CargaMasiva { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaActualizacion { get; set; }

    }
}
