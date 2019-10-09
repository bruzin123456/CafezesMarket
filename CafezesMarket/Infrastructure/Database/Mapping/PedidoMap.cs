using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class PedidoMap : IEntityTypeConfiguration<Pedido>
    {
        public void Configure(EntityTypeBuilder<Pedido> builder)
        {
            builder.ToTable("pedido");
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

            builder.Property(model => model.SituacaoId)
                .HasColumnName("situacao_id")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(model => model.Emissao)
                .HasColumnName("emissao")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.HasOne(model => model.Cliente)
                .WithMany(model => model.Pedidos)
                .HasForeignKey(pedido => pedido.ClienteId);

            builder.HasOne(model => model.Situacao)
                .WithMany()
                .HasForeignKey(pedido => pedido.SituacaoId);

            builder.HasMany(model => model.Itens)
                .WithOne()
                .HasForeignKey(item => item.PedidoId);
        }
    }
}
