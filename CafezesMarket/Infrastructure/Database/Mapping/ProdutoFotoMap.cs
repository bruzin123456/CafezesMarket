using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class ProdutoFotoMap : IEntityTypeConfiguration<ProdutoFoto>
    {
        public void Configure(EntityTypeBuilder<ProdutoFoto> builder)
        {
            builder.ToTable("produto_foto");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(model => model.ProdutoId)
                .HasColumnName("produto_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(model => model.Caminho)
                .HasColumnName("caminho")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
