using Microsoft.EntityFrameworkCore;
using Moonlay.Core.Models;
using Moonlay.MasterData.WebApi.Models;

namespace Moonlay.MasterData.WebApi.Db
{
    public class MyDbContext : MoonlayDbContext
    {
        public MyDbContext(DbContextOptions<MyDbContext> options, IDbTrailContext trailContext, ISignInService signInService) : base(options, trailContext, signInService)
        {
        }

        public DbSet<Customer> Customers => Set<Customer>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customer>(etb =>
            {
                etb.DefaultEntity();
            });
        }
    }

    public class MyDbTrailContext : MoonlayDbTrailContext
    {
        public MyDbTrailContext(DbContextOptions<MyDbTrailContext> options) : base(options)
        {
        }

        public DbSet<CustomerTrail> CustomerTrails => Set<CustomerTrail>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerTrail>(etb =>
            {
                etb.HasKey(k => k.Id);
            });
        }
    }
}