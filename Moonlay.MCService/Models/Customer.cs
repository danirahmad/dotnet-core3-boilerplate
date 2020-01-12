using Moonlay.Core.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Moonlay.MCServiceWebApi.Models
{
    public class Customer : Entity
    {
        public Customer(Guid id) : base(id) { }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }

        public override bool IsHasTestMode() => true;

        public override bool IsSoftDelete() => true;

        public override object ToTrail()
        {
            return new CustomerTrail(this)
            {
                CustomerId = Id,
                FirstName = FirstName,
                LastName = LastName
            };
        }
    }

    public class CustomerTrail : EntityTrail
    {
        public CustomerTrail()
        {
        }

        public CustomerTrail(Customer entity) : base(entity)
        {
            this.CustomerId = entity.Id;
            this.FirstName = entity.FirstName;
            this.LastName = entity.LastName;
        }

        public Guid CustomerId { get; set; }

        [MaxLength(64)]
        public string FirstName { get; set; }

        [MaxLength(64)]
        public string LastName { get; set; }
    }
}