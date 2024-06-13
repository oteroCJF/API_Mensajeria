using Mensajeria.Domain.DFirmantes;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class FirmantesConfiguration
    {
        public FirmantesConfiguration(EntityTypeBuilder<Firmante> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
