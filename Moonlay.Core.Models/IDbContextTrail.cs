using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.Core.Models
{
    public interface IDbTrailContext : IDisposable
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

        //EntityEntry Add(object entity);

        Task AddRangeAsync(IEnumerable<object> entities, CancellationToken cancellationToken = default);
    }
}