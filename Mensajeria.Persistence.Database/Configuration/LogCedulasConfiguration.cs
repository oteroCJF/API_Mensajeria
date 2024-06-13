using Mensajeria.Domain.DHistorial;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class LogCedulasConfiguration
    {
        public LogCedulasConfiguration(EntityTypeBuilder<LogCedula> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
