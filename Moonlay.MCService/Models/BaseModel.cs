using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlay.MCService.Models
{
    public interface IToTrail<T>
    {
        T ToTrail();
    }

    public abstract class BaseModel : IToTrail<object>
    {
        public DateTimeOffset CreatedAt { get; set; }

        [MaxLength(64)]
        public string CreatedBy { get; set; }

        [MaxLength(64)]
        public string UpdatedBy { get; set; }

        public DateTimeOffset UpdatedAt { get; set; }

        public bool Deleted { get; set; }

        // Tested indicate for the record running in testing mode
        public bool Tested { get; set; }

        public abstract object ToTrail();
    }

    public abstract class BaseModelTrail
    {
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