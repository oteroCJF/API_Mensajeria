using Mensajeria.Domain.DHistorial;
using Mensajeria.Domain.DHistorialOficios;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class LogOficiosConfiguration
    {
        public LogOficiosConfiguration(EntityTypeBuilder<LogOficio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
