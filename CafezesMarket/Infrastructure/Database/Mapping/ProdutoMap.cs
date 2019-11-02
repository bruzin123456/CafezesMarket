using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class ProdutoMap : IEntityTypeConfiguration<Produto>
    {
        public void Configure(EntityTypeBuilder<Produto> builder)
        {
            builder.ToTable("produto");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(model => model.Titulo)
                .HasColumnName("titulo")
                .HasColumnType("varchar(128)")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(model => model.Descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar(2048)")
                .HasMaxLength(2048);

            builder.Property(model => model.Categoria)
                .HasColumnName("categoria")
                .HasColumnType("varchar(128)")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(model => model.Preco)
                .HasColumnName("preco")
                .HasColumnType("decimal")
                .IsRequired();

            builder.Property(model => model.Quantidade)
                .HasColumnName("quantidade")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(model => model.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("bit")
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasMany(model => model.Fotos)
                .WithOne()
                .HasForeignKey(foto => foto.ProdutoId);

            builder.HasMany(model => model.PedidosItems)
                .WithOne(item => item.Produto)
                .HasForeignKey(item => item.ProdutoId);
        }
    }
}
