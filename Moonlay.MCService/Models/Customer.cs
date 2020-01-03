using Moonlay.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlay.MCService.Models
{
    public class Customer : Entity
    {
        public Customer(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }

        public override object ToTrail()
        {
            return new CustomerTrail
            {
                CustomerId = Id,
                FirstName = FirstName,
                LastName = LastName,

                CreatedAt = CreatedAt,
                CreatedBy = CreatedBy,
                UpdatedAt = UpdatedAt,
                UpdatedBy = UpdatedBy,
                Deleted = Deleted,
                Tested = Tested
            };
        }
    }

    public class CustomerTrail : EntityTrail
    {
        public Int64 Id { get; set; }

        public Guid CustomerId { get; set; }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }
    }
}