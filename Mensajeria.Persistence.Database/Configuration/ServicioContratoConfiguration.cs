using Mensajeria.Domain.DContratos;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class ServicioContratoConfiguration
    {
        public ServicioContratoConfiguration(EntityTypeBuilder<ServicioContrato> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
        }
    }
}
