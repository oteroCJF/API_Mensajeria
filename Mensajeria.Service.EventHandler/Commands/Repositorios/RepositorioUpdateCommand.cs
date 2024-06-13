using MediatR;
using Mensajeria.Domain.DRepositorios;
using System;

namespace Mensajeria.Service.EventHandler.Commands.Repositorios
{
    public class RepositorioUpdateCommand : IRequest<Repositorio>
    {
        public int Id { get; set; }
        public int ContratoId { get; set; }
        public string UsuarioId { get; set; }
        public int MesId { get; set; }
        public int Anio { get; set; }
        public int EstatusId { get; set; }
        public DateTime? FechaActualizacion { get; set; }
    }
}
