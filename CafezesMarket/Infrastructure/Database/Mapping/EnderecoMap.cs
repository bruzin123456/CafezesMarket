using CafezesMarket.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CafezesMarket.Infrastructure.Database.Mapping
{
    public class EnderecoMap : IEntityTypeConfiguration<Endereco>
    {
        public void Configure(EntityTypeBuilder<Endereco> builder)
        {
            builder.ToTable("endereco");
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

            builder.Property(model => model.EstadoId)
                .HasColumnName("estado_id")
                .HasColumnType("bigint")
                .IsRequired();

            builder.Property(model => model.Apelido)
                .HasColumnName("apelido")
                .HasColumnType("varchar(128)")
                .HasMaxLength(128)
                .IsRequired();

            builder.Property(model => model.Logradouro)
                .HasColumnName("logradouro")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(model => model.Numero)
                .HasColumnName("numero")
                .HasColumnType("int")
                .IsRequired();

            builder.Property(model => model.Complemento)
                .HasColumnName("complemento")
                .HasColumnType("varchar(1024)")
                .HasMaxLength(1024);

            builder.Property(model => model.Cidade)
                .HasColumnName("cidade")
                .HasColumnType("varchar(512)")
                .HasMaxLength(512)
                .IsRequired();

            builder.Property(model => model.Ativo)
                .HasColumnName("ativo")
                .HasColumnType("bit")
                .HasDefaultValue(true)
                .IsRequired();

            builder.HasOne(model => model.Estado)
                .WithMany()
                .HasForeignKey(endereco => endereco.EstadoId);
        }
    }
}
