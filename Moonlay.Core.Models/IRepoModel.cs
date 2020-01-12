using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Moonlay.Core.Models
{
    public interface IRepoModel<TModel, TModelTrail> 
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
