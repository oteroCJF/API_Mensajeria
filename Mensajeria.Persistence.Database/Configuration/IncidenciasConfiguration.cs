using Mensajeria.Domain.DIncidencias;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class IncidenciasConfiguration
    {
        public IncidenciasConfiguration(EntityTypeBuilder<Incidencia> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.EstatusId).HasDefaultValue(0);
        }
    }
}
