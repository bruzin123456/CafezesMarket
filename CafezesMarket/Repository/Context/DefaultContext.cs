using CafezesMarket.Repository.Mapping;
using Microsoft.EntityFrameworkCore;

namespace CafezesMarket.Repository.Context
{
    public class DefaultContext : DbContext
    {
        public DefaultContext ()
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new ClienteMap());

            base.OnModelCreating(modelBuilder);
        }
    }
}