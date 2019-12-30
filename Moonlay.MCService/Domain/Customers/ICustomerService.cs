using System;
using System.Collections.Generic;
using Moonlay.MCService.Models;

namespace Moonlay.MCService.Customers
{
    public interface ICustomerService
    {
        Customer NewCustomer(string firstName, string lastName);
        List<Customer> Search(Func<Customer, bool> criteria = null, int page = 0, int size = 25);
    }

}