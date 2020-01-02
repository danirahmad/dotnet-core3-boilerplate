using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Moonlay.MCService.Controllers.DTO;
using Moonlay.MCService.Customers;

namespace Moonlay.MCService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet()]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            var result = (await _customerService.SearchAsync())
                .Select(o=>new CustomerDto(o))
                .ToList();

            return Ok(result);
        }
    }
}