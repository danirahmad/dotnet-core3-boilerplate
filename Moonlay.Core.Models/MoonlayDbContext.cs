using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.Core.Models
{
    public static class ModelBuilderHelper
    {
        public static void SetGlobalQuery<T>(this Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<T> etb) where T : Entity
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
            var entities = this.ChangeTracker.Entries<Entity>();
            var now = DateTime.Now;
            var currentUser = _signInService.CurrentUser;
            var isDemo = _signInService.Demo;

            foreach (var item in entities)
            {
                if (item.State == EntityState.Added)
                {
                    item.Entity.CreatedAt = now;
                    item.Entity.CreatedBy = currentUser;
                    item.Entity.UpdatedAt = now;
                    item.Entity.UpdatedBy = currentUser;
                    item.Entity.Deleted = false;

                    if (item.Entity.IsHasTestMode())
                        item.Entity.Tested = isDemo;
                    else
                        item.Entity.Tested = false;
                }
                else if (item.State == EntityState.Modified)
                {
                    item.Entity.UpdatedAt = now;
                    item.Entity.UpdatedBy = currentUser;
                }
                else if (item.Entity.IsSoftDelete())
                {
                    item.Entity.Deleted = true;
                    item.Entity.UpdatedAt = now;
                    item.Entity.UpdatedBy = currentUser;
                    item.State = EntityState.Modified;
                }
            }

            var trailEntities = entities.Select(o => o.Entity.ToTrail());

            foreach (var trail in trailEntities)
            {
                _trailContext.Add(trail);
            }

            var result = await base.SaveChangesAsync(cancellationToken);

            await _trailContext.SaveChangesAsync(cancellationToken);

            return result;
        }
    }

    public abstract class MoonlayDbTrailContext : DbContext, IDbTrailContext
    {
        public MoonlayDbTrailContext(DbContextOptions options) : base(options) { }
    }
}