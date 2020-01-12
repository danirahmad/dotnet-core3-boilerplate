using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moonlay.MCService.Controllers.DTO;
using Moonlay.MCService.Customers;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moonlay.MCService.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;

        public CustomersController(ICustomerService customerService, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
        }

        [HttpGet()]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            var result = (await _customerService.SearchAsync())
                .Select(o => new CustomerDto(o))
                .ToList();

            return Ok(result);
        }

    }
}