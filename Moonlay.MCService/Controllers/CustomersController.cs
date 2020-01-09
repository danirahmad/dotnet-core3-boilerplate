using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.AspNetCore.Mvc;
using Moonlay.MCService.Controllers.DTO;
using Moonlay.MCService.Customers;
using Microsoft.Extensions.DependencyInjection;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;
using Moonlay.MCService.Producers;

namespace Moonlay.MCService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ILogger<CustomersController> _logger;
        private readonly INewCustomerProducer _NewCustomerProducer;

        public CustomersController(ICustomerService customerService, IServiceProvider provider, ILogger<CustomersController> logger)
        {
            _customerService = customerService;
            _logger = logger;
            _NewCustomerProducer = provider.GetRequiredService<INewCustomerProducer>();
        }

        [HttpGet()]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            var result = (await _customerService.SearchAsync())
                .Select(o => new CustomerDto(o))
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(NewCustomerDto form)
        {
            var message = new MessageTypes.LogMessage
            {
                IP = "192.168.0.1",
                Message = "a test message 2",
                Severity = MessageTypes.LogLevel.Info,
                Tags = new Dictionary<string, string> { { "location", "CA" } }
            };

            await _NewCustomerProducer.Publish(Guid.NewGuid().ToString(), message);

            return Ok(new CustomerDto(new Models.Customer(Guid.NewGuid()) { FirstName = form.FirstName, LastName = form.LastName }));
        }
    }
}