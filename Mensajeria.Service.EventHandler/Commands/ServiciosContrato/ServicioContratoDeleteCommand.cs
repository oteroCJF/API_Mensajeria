using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.EventHandler.Commands.ServiciosContrato
{
    public class ServicioContratoDeleteCommand : IRequest<int>
    {
        public int ContratoId { get; set; }
        public int ServicioId { get; set; }
    }
}
