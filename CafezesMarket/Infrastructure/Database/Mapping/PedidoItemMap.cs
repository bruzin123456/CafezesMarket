using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class PedidoItemMap : IEntityTypeConfiguration<PedidoItem>
    {
        public void Configure(EntityTypeBuilder<PedidoItem> builder)
        {
            builder.ToTable("pedido_item");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(model => model.PedidoId)
                .HasColumnName("pedido_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(model => model.ProdutoId)
                .HasColumnName("produto_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(model => model.Preco)
                .HasColumnName("preco")
                .HasColumnType("decimal")
                .IsRequired();

            builder.Property(model => model.Desconto)
                .HasColumnName("desconto")
                .HasColumnType("decimal")
                .IsRequired();

            builder.Property(model => model.Quantidade)
                .HasColumnName("quantidade")
                .HasColumnType("int")
                .IsRequired();

            builder.HasOne(model => model.Pedido)
                .WithMany(pedido => pedido.Itens)
                .HasForeignKey(item => item.PedidoId);

            builder.HasOne(model => model.Produto)
                .WithMany(produto => produto.PedidosItems)
                .HasForeignKey(item => item.ProdutoId);
        }
    }
}
