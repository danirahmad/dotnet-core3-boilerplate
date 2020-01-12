using Moonlay.Core.Models;
using Moonlay.MCServiceWebApi.Models;

namespace Moonlay.MCServiceWebApi.Customers
{
    public interface ICustomerRepository : IRepoEntity<Customer, CustomerTrail>
    {

    }
}