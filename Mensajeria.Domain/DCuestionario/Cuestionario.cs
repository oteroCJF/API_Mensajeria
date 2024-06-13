using System;

namespace Mensajeria.Domain.DCuestionario
{
    public class Cuestionario
    {
        public int Id { get; set; }
        public int NoPregunta { get; set; }
        public string Abreviacion { get; set; }
        public string Pregunta { get; set; }
        public string Ayuda { get; set; }
        public string Concepto { get; set; }
        public string Botones { get; set; }
        public string Icono { get; set; }
        public bool NoAplica { get; set; }
        public bool Incidencias { get; set; }
        public bool CargaMasiva { get; set; }
        public Nullable<DateTime> FechaCreacion { get; set; }
        public Nullable<DateTime> FechaActualizacion { get; set; }
        public Nullable<DateTime> FechaEliminacion { get; set; }
    }
}
