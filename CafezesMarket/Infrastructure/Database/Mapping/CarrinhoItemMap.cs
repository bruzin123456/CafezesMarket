using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class CarrinhoItemMap : IEntityTypeConfiguration<CarrinhoItem>
    {
        public void Configure(EntityTypeBuilder<CarrinhoItem> builder)
        {
            builder.ToTable("carrinho_item");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(model => model.ClienteId)
                .HasColumnName("cliente_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(model => model.ProdutoId)
                .HasColumnName("produto_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(model => model.Quantidade)
                .HasColumnName("quantidade")
                .HasColumnType("int")
                .IsRequired();

            builder.HasOne(model => model.Produto)
                .WithMany()
                .HasForeignKey(item => item.ProdutoId);

            builder.HasOne(model => model.Cliente)
                .WithMany(model => model.CarrinhoItens)
                .HasForeignKey(carrinhoItem => carrinhoItem.ClienteId);
        }
    }
}
