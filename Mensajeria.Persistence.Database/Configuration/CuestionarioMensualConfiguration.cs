using Mensajeria.Domain.DCuestionario;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class CuestionarioMensualConfiguration
    {
        public CuestionarioMensualConfiguration(EntityTypeBuilder<CuestionarioMensual> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.CuestionarioId, x.ContratoId, x.Consecutivo, x.Anio, x.MesId });
        }
    }
}
