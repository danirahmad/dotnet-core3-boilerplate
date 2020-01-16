using Moonlay.Core.Models;
using Moonlay.MasterData.WebApi.Models;

namespace Moonlay.MasterData.WebApi.Customers
{
    public interface ICustomerRepository : IRepoEntity<Customer, CustomerTrail>
    {

    }
}