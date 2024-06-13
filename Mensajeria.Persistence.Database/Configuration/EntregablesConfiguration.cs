using Mensajeria.Domain.DEntregables;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class EntregablesConfiguration
    {
        public EntregablesConfiguration(EntityTypeBuilder<Entregable> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.EstatusId).HasDefaultValue(1);
            entityBuilder.Property(x => x.UsuarioId).HasDefaultValue("ffc1e09b-6dd5-487f-9d95-999999999999");
        }
    }
}
