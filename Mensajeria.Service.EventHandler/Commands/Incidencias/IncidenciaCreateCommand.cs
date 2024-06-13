using MediatR;
using Mensajeria.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.EventHandler.Commands.Incidencias
{
    public class IncidenciaCreateCommand : IRequest<Incidencia>
    {
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int IndemnizacionId { get; set; }
        public int EstatusId { get; set; }
        public int Pregunta { get; set; }
        public string NumeroGuia { get; set; }
        public string CodigoRastreo { get; set; }
        public decimal Sobrepeso { get; set; }
        public string TipoServicio { get; set; }
        public string Acuse { get; set; }
        public int TotalAcuses { get; set; }
        public IFormFile Acta { get; set; }
        public IFormFile Escrito { get; set; }
        public IFormFile Comprobante { get; set; }
        public bool Penalizable { get; set; }
        public decimal MontoPenalizacion { get; set; }
        public DateTime? FechaProgramada { get; set; }
        public DateTime? FechaEntrega { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public string? Observaciones { get; set; }

        public string TipoIncidencia { get; set; }

        //Variables para el acta de Robo/Extravio
        public string Mes { get; set; }
        public string Folio { get; set; }
        public int Anio { get; set; }
    }
}
