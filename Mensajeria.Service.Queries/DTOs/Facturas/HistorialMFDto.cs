﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.Queries.DTOs.Facturas
{
    public class HistorialMFDto
    {
        public int Anio { get; set; }
        public string Mes { get; set; }
        public int RepositorioId { get; set; }
        public int InmuebleId { get; set; }
        public string UsuarioId { get; set; }
        public string TipoArchivo { get; set; }
        public string Facturaicion { get; set; }
        public string ArchivoXML { get; set; }
        public string ArchivoPDF { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
