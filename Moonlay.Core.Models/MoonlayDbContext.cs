using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.Core.Models
{
    public static class ModelBuilderHelper
    {
        public static void DefaultEntity<T>(this Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> etb) where T : Entity
        {
            etb.HasKey(e => e.Id);
            etb.HasQueryFilter(e => !e.Deleted);
        }
    }

    public abstract class MoonlayDbContext : DbContext, IDbContext
    {
        private readonly IDbTrailContext _trailContext;
        private readonly ISignInService _signInService;

        public MoonlayDbContext(DbContextOptions options, IDbTrailContext trailContext, ISignInService signInService) : base(options)
        {
            _trailContext = trailContext;
            _signInService = signInService;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entities = this.ChangeTracker.Entries<Entity>().Where(o => o.State == EntityState.Added || o.State == EntityState.Modified || o.State == EntityState.Deleted);
            var now = DateTime.Now;
            var currentUser = _signInService.CurrentUser;
            var isDemo = _signInService.Demo;

            Parallel.ForEach(entities, item =>
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedAt = now;
                    item.Entity.CreatedBy = currentUser;
                    item.Entity.UpdatedAt = now;
                    item.Entity.UpdatedBy = currentUser;
                    item.Entity.Deleted = false;

                    item.Entity.Tested = item.Entity.IsHasTestMode() ? isDemo : false;
                }
                else if (item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedAt = now;
                    item.Entity.UpdatedBy = currentUser;
                }
                else if (item.State == EntityState.Deleted && item.Entity.IsSoftDelete())
                {
                    item.Entity.Deleted = true;
                    item.Entity.UpdatedAt = now;
                    item.Entity.UpdatedBy = currentUser;
                    item.State = EntityState.Modified;
                }
            });

            var trailEntities = entities.Select(o => o.Entity.ToTrail()).ToList();

            var result = await base.SaveChangesAsync(cancellationToken);

            if (trailEntities.Any())
            {
                await _trailContext.AddRangeAsync(trailEntities, cancellationToken);

                await _trailContext.SaveChangesAsync(cancellationToken);
            }


            return result;
        }
    }

    public abstract class MoonlayDbTrailContext : DbContext, IDbTrailContext
    {
        public MoonlayDbTrailContext(DbContextOptions options) : base(options) { }
    }
}