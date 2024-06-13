using Mensajeria.Domain.DHistorialEntregables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class LogEntregablesConfiguration
    {
        public LogEntregablesConfiguration(EntityTypeBuilder<LogEntregable> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
