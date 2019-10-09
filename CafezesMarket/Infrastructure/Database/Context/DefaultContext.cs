using CafezesMarket.Infrastructure.Database.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CafezesMarket.Infrastructure.Database.Context
{
    public class DefaultContext : DbContext
    {
        public DefaultContext (DbContextOptions<DefaultContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CredencialMap())
                .ApplyConfiguration(new EstadoMap())
                .ApplyConfiguration(new EnderecoMap())
                .ApplyConfiguration(new ClienteMap())
                .ApplyConfiguration(new ProdutoFotoMap())
                .ApplyConfiguration(new ProdutoMap())
                .ApplyConfiguration(new PedidoSituacaoMap())
                .ApplyConfiguration(new PedidoMap())
                .ApplyConfiguration(new PedidoItemMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}