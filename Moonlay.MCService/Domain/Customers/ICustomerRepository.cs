using System;
using Microsoft.EntityFrameworkCore;
using Moonlay.MCService.Models;

namespace Moonlay.MCService.Customers
{
    public interface ICustomerRepository
    {
        DbSet<Customer> DbSet { get; }

        DbSet<CustomerTrail> DbSetTrail { get; }

        Customer With(Guid id);
    }
}