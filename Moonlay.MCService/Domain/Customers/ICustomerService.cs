using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moonlay.MCService.Models;

namespace Moonlay.MCService.Customers
{
    public interface ICustomerService
    {
        Task<Customer> NewCustomerAsync(string firstName, string lastName);

        Task<List<Customer>> SearchAsync(Func<Customer, bool> criteria = null, int page = 0, int size = 25);

        Task<List<CustomerTrail>> LogsAsync(Guid id);

        Task<Customer> UpdateProfileAsync(Guid id, string firstName, string lastName);
    }

}