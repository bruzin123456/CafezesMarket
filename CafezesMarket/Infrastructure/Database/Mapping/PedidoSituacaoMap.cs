using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class PedidoSituacaoMap : IEntityTypeConfiguration<PedidoSituacao>
    {
        public void Configure(EntityTypeBuilder<PedidoSituacao> builder)
        {
            builder.ToTable("pedido_situacao");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .HasColumnName("id")
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(model => model.Descricao)
                .HasColumnName("descricao")
                .HasColumnType("varchar(256)")
                .HasMaxLength(256)
                .IsRequired();
        }
    }
}
