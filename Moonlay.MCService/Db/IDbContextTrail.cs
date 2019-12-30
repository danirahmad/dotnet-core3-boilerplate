using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Moonlay.MCService.Db
{
    public interface IDbTrailContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        
        int SaveChanges();

        Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default);
    }
}