using Mensajeria.Domain.DContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class ContratosConfiguration
    {
        public ContratosConfiguration(EntityTypeBuilder<Contrato> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
