using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Moonlay.Core.Models
{
    public class RepoEntity<TModel, TModelTrail> : IRepoEntity<TModel, TModelTrail>
        where TModel : Entity
        where TModelTrail : EntityTrail
    {
        public DbSet<TModel> DbSet { get; }
        public DbSet<TModelTrail> DbSetTrail { get; }
        public string CurrentUser { get; }
        public bool IsCurrentUserDemo { get; }

        public RepoEntity(IDbContext dbContext, IDbTrailContext dbTrailContext, ISignInService signIn)
        {
            DbSet = dbContext.Set<TModel>();
            DbSetTrail = dbTrailContext.Set<TModelTrail>();
            CurrentUser = signIn.CurrentUser;
            IsCurrentUserDemo = signIn.Demo;
        }

        public TModel With(Guid id)
        {
            var r = DbSet.FirstOrDefault(x => x.Id == id);
            if (r == null) throw new RecordNotFoundException($"{nameof(TModel)} {id} not found");

            return r;
        }

        public TModelTrail WithTrail(long id)
        {
            var r = DbSetTrail.FirstOrDefault(o => o.Id == id);
            if (r == null) throw new RecordNotFoundException($"{nameof(TModelTrail)} {id} not found");

            return r;
        }
    }
}
