﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Domain.DHistorial
{
    public class LogCedula
    {
        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int EstatusId { get; set; }
        public string UsuarioId { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
    }
}
