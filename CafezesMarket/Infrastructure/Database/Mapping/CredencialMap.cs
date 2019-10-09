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

            builder.Property(model => model.Usuario)
                .HasColumnName("usuario")
                .HasColumnType("varchar(128)")
                .IsRequired();

            builder.Property(model => model.Senha)
                .HasColumnName("senha")
                .HasColumnType("varchar(2048)")
                .IsRequired();

            builder.Property(model => model.Salt)
                .HasColumnName("salt")
                .HasColumnType("varchar(512)");

            builder.HasIndex(model => model.Usuario)
                .IsUnique();

            builder.HasIndex(model => new { model.Usuario, model.Senha })
                .HasName("IDX_credencial_usuario_senha");
        }
    }
}
