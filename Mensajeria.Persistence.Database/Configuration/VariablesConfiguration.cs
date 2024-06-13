using Mensajeria.Domain.DVariables;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class VariablesConfiguration
    {
        public VariablesConfiguration(EntityTypeBuilder<Variables> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
