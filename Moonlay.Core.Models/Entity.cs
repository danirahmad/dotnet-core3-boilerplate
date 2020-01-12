using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlay.Core.Models
{
    public abstract class Entity
    {
        public Entity(Guid id)
        {
            Id = id;
        }

        [Key]
        public Guid Id { get; }

        public DateTimeOffset CreatedAt { get; set; }

        [MaxLength(64)]
        public string CreatedBy { get; set; }

        [MaxLength(64)]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        /// <summary>
        /// Enable Soft Delete
        /// </summary>
        public bool Deleted { get; set; }

        // Tested indicate for the record running in testing mode
        public bool Tested { get; set; }

        public abstract object ToTrail();

        public abstract bool IsSoftDelete();

        public abstract bool IsHasTestMode();
    }

    public abstract class EntityTrail
    {
        public EntityTrail() { }

        public EntityTrail(Entity entity)
        {
            CreatedAt = entity.CreatedAt;
            UpdatedAt = entity.UpdatedAt;
            CreatedBy = entity.UpdatedBy;
            UpdatedBy = entity.UpdatedBy;

            Deleted = entity.Deleted;
            Tested = entity.Tested;
        }

        public Int64 Id { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        [MaxLength(64)]
        public string CreatedBy { get; set; }

        [MaxLength(64)]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        // Tested indicate for the record running in testing mode
        public bool Tested { get; set; }
    }

    public static class BaseModelExt
    {
        //public static T MarkTested<T>(this T m) where T : BaseModel
        //{
        //    m.Tested = true;
        //    return m;
        //}

        //public static T UnMarkTested<T>(this T m) where T : BaseModel
        //{
        //    m.Tested = false;
        //    return m;
        //}

        //public static T MarkDeleted<T>(this T m) where T : BaseModel
        //{
        //    m.Deleted = true;
        //    return m;
        //}

        //public static T UnMarkDeleted<T>(this T m) where T : BaseModel
        //{
        //    m.Deleted = true;
        //    return m;
        //}
    }
}