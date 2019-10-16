using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class ClienteMap : IEntityTypeConfiguration<Cliente>
    {
        public void Configure(EntityTypeBuilder<Cliente> builder)
        {
            builder.ToTable("cliente");
            builder.HasKey(model => model.Id);

            builder.Property(model => model.Id)
                .HasColumnName("id")
                .HasColumnType("bigint")
                .ValueGeneratedOnAdd()
                .IsRequired();

            builder.Property(model => model.Nome)
                .HasColumnName("nome")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(model => model.Cpf)
                .HasColumnName("cpf")
                .HasColumnType("varchar(11)")
                .HasMaxLength(11)
                .IsRequired();

            builder.Property(model => model.Nascimento)
                .HasColumnName("nascimento")
                .HasColumnType("datetime2")
                .IsRequired();

            builder.Property(model => model.Email)
                .HasColumnName("email")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512)
                .IsRequired();

            builder.HasIndex(model => model.Email)
                .HasName("IDX_cliente_email")
                .IsUnique();

            builder.HasOne(model => model.Credencial)
                .WithOne()
                .HasForeignKey<Credencial>(credencial => credencial.ClienteId);

            builder.HasMany(model => model.Enderecos)
                .WithOne()
                .HasForeignKey(endereco => endereco.ClienteId);

            builder.HasMany(model => model.Pedidos)
                .WithOne(model => model.Cliente)
                .HasForeignKey(pedido => pedido.ClienteId);
        }
    }
}