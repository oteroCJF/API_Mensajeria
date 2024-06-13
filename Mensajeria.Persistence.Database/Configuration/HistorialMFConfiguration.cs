using Mensajeria.Domain.DFacturas;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mensajeria.Persistence.Database.Configuration
{
    public class HistorialMFConfiguration
    {
        public HistorialMFConfiguration(EntityTypeBuilder<HistorialMF> entityBuilder)
        {
            entityBuilder.HasKey(x => new { x.Anio, x.Mes, x.RepositorioId, x.InmuebleId, x.UsuarioId, x.TipoArchivo, x.Facturacion, x.ArchivoXML, x.ArchivoPDF, x.Observaciones});
        }
    }
}
