using Mensajeria.Domain.DOficios;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class OficiosConfiguration
    {
        public OficiosConfiguration(EntityTypeBuilder<Oficio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.EstatusId).HasDefaultValue(2);
        }
    }
}
