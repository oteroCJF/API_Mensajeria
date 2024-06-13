using Mensajeria.Domain.DCedulaEvaluacion;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class CedulaEvaluacionConfiguration
    {
        public CedulaEvaluacionConfiguration(EntityTypeBuilder<CedulaEvaluacion> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
