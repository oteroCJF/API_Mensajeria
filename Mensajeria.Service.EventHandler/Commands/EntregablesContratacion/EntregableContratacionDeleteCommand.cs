using MediatR;
using System;

namespace Mensajeria.Service.EventHandler.Commands.EntregableContratacion
{
    public class EntregableContratacionDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
        public System.Nullable<DateTime> FechaEliminacion { get; set; }
    }
}
