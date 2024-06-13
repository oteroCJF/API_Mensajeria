using MediatR;
using Mensajeria.Domain.DIncidencias;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Mensajeria.Service.EventHandler.Commands.Incidencias
{
    public class IncidenciaExcelCreateCommand : IRequest<List<Incidencia>>
    {
        public string Folio { get; set; }
        public string UsuarioId { get; set; }
        public int CedulaEvaluacionId { get; set; }
        public int IncidenciaId { get; set; }
        public int Pregunta { get; set; }
        public IFormFile Excel { get; set; }
    }
}
