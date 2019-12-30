using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlay.MCService.Models
{
    public class Customer : BaseModel
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

        public CustomerTrail ToTrail()
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

    public class CustomerTrail : BaseModel
    {
        public Int64 Id { get; set; }

        public Guid CustomerId { get; set; }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }
    }
}