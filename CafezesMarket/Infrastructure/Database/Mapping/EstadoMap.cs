using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class EstadoMap : IEntityTypeConfiguration<Estado>
    {
        public void Configure(EntityTypeBuilder<Estado> builder)
        {
            builder.ToTable("estado");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
               .HasColumnName("id")
               .HasColumnType("int")
               .ValueGeneratedOnAdd()
               .IsRequired();

            builder.Property(model => model.Sigla)
                .HasColumnName("sigla")
                .HasColumnType("char(2)")
                .HasMaxLength(2)
                .IsFixedLength()
                .IsRequired();

            builder.HasIndex(model => model.Sigla)
                .IsUnique();
        }
    }
}
