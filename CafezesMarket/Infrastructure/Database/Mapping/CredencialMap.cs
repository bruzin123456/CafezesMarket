using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class CredencialMap : IEntityTypeConfiguration<Credencial>
    {
        public void Configure(EntityTypeBuilder<Credencial> builder)
        {
            builder.ToTable("credencial");
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

            builder.Property(model => model.Senha)
                .HasColumnName("senha")
                .HasColumnType("varchar(1024)")
                .IsRequired();

            builder.Property(model => model.Salt)
                .HasColumnName("salt")
                .HasColumnType("varchar(512)");

            builder.Property(model => model.Erros)
                .HasColumnName("erros")
                .HasColumnType("int")
                .HasDefaultValue(0)
                .IsRequired();

            builder.Property(model => model.UltimoErro)
                .HasColumnName("ultimo_erro")
                .HasColumnType("datetime2");
        }
    }
}
