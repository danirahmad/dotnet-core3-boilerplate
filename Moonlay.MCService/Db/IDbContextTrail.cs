using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace Moonlay.MCService.Db
{
    public interface IDbTrailContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        
        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        EntityEntry Add(object entity);
    }
}