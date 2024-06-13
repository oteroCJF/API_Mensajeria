using MediatR;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.EventHandler.Commands.Incidencias
{
    public class SoportePagoUpdateCommand : IRequest<int>
    {
        public string Folio { get; set; }
        public int Anio { get; set; }
        public string Mes { get; set; }
        public string UsuarioId { get; set; }
        public IFormFile TXT { get; set; }
        public string Cedulas { get; set; }
        public List<CedulaSoporteCommand> CedulasEvaluacion { get; set; } = new List<CedulaSoporteCommand>();
    }
}
