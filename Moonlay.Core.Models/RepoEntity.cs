using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace Moonlay.Core.Models
{
    public class RepoEntity<TModel, TModelTrail> : IRepoEntity<TModel, TModelTrail>
        where TModel : Entity
        where TModelTrail : EntityTrail
    {

        private readonly ISignInService _signIn;
        private readonly IDbContext _dbContext;
        private readonly IDbTrailContext _dbTrailContext;

        public DbSet<TModel> DbSet => _dbContext.Set<TModel>();
        public DbSet<TModelTrail> DbSetTrail => _dbTrailContext.Set<TModelTrail>();

        public string CurrentUser => _signIn.CurrentUser;
        public bool IsCurrentUserDemo => _signIn.Demo;

        public RepoEntity(IDbContext dbContext, IDbTrailContext dbTrailContext, ISignInService signIn)
        {
            _dbContext = dbContext;
            _dbTrailContext = dbTrailContext;
            _signIn = signIn;
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
