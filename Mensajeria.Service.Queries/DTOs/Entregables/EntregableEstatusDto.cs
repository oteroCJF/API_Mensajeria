using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Service.Queries.DTOs.Entregables
{
    public class EntregableEstatusDto
    {
        public int EntregableId { get; set; }
        public int EstatusId { get; set; }
        public bool Multiple { get; set; }
    }
}
