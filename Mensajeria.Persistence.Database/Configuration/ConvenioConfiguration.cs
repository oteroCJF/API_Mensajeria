using Mensajeria.Domain.DContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class ConvenioConfiguration
    {
        public ConvenioConfiguration(EntityTypeBuilder<Convenio> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
