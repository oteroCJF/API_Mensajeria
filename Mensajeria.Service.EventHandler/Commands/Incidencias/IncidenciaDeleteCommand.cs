using MediatR;
using Mensajeria.Domain.DIncidencias;
using System;

namespace Mensajeria.Service.EventHandler.Commands.Incidencias
{
    public class IncidenciaDeleteCommand : IRequest<int>
    {
        public int Id { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int Pregunta { get; set; }
        public DateTime? FechaEliminacion { get; set; }
        public string TipoIncidencia { get; set; }

        //Variables para el acta de Robo/Extravio
        public string Mes { get; set; }
        public string Folio { get; set; }
        public int Anio { get; set; }
    }
}
