using MediatR;
using Mensajeria.Domain.DCedulaEvaluacion;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.EventHandler.Commands.CedulasEvaluacion.DBloquearCedula
{
    public class DBloquearCedulaUpdateCommand : IRequest<CedulaEvaluacion>
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }
        public int EstatusId { get; set; }
        public int RepositorioId { get; set; }
        public int EFacturaId { get; set; }
        public bool Bloqueada { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaActualizacion { get; set; }
    }
}
