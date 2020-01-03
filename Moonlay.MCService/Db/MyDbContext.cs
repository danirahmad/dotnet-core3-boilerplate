using Microsoft.EntityFrameworkCore;
using Moonlay.MCService.Models;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MCService.Db
{
    public class MyDbContext : DbContext, IDbContext
    {
        private readonly IDbTrailContext _trailContext;

        public DbSet<Customer> Customers => Set<Customer>();

        public MyDbContext(DbContextOptions<MyDbContext> options, IDbTrailContext trailContext) : base(options)
        {
            _trailContext = trailContext;
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Customer>(etb =>
            {
                etb.HasKey(k => k.Id);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = this.ChangeTracker.Entries<BaseModel>().Select(o => o.Entity.ToTrail());
            foreach(var entity in entities)
            {
                _trailContext.Add(entity);
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await _trailContext.SaveChangesAsync(cancellationToken);

            return result;
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