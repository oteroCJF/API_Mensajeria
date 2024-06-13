using Mensajeria.Domain.DContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class RubroConvenioConfiguration
    {
        public RubroConvenioConfiguration(EntityTypeBuilder<RubroConvenio> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.RubroId, x.ConvenioId });
        }
    }
}
