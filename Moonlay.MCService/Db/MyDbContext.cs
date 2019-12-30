using Microsoft.EntityFrameworkCore;
using Moonlay.MCService.Models;

namespace Moonlay.MCService.Db
{
    public class MyDbContext : DbContext, IDbContext
    {
        public DbSet<Customer> Customers => Set<Customer>();

        public MyDbContext(DbContextOptions<MyDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(etb =>
            {
                etb.HasKey(k => k.Id);
            });
        }
    }

    public class MyDbTrailContext : DbContext, IDbTrailContext
    {
        public DbSet<CustomerTrail> CustomerTrails => Set<CustomerTrail>();

        public MyDbTrailContext(DbContextOptions<MyDbTrailContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerTrail>(etb =>
            {
                etb.HasKey(k => k.Id);
            });
        }
    }
}