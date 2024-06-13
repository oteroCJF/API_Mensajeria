using Mensajeria.Domain.DCuestionario;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class CuestionarioConfiguration
    {
        public CuestionarioConfiguration(EntityTypeBuilder<Cuestionario> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
