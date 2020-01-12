using Microsoft.EntityFrameworkCore;
using System;

namespace Moonlay.Core.Models
{
    public interface IRepoEntity<TModel, TModelTrail>
        where TModel : Entity
        where TModelTrail : EntityTrail
    {
        DbSet<TModel> DbSet { get; }

        DbSet<TModelTrail> DbSetTrail { get; }

        TModel With(Guid id);

        string CurrentUser { get; }

        bool IsCurrentUserDemo { get; }
    }
}
