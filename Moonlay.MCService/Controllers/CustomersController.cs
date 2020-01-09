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

namespace Moonlay.MCService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        private readonly ProducerConfig _kafkaProducerConfig;
        private readonly ISchemaRegistryClient _kafkaSchemaRegistry;
        private readonly IProducer<string, MessageTypes.LogMessage> _NewCustomerProducer;

        public CustomersController(ICustomerService customerService, IServiceProvider provider)
        {
            _customerService = customerService;

            _kafkaProducerConfig = provider.GetService<ProducerConfig>();
            _kafkaSchemaRegistry = provider.GetService<ISchemaRegistryClient>();
            _NewCustomerProducer = provider.GetRequiredService<IProducer<string, MessageTypes.LogMessage>>();
        }

        [HttpGet()]
        public async Task<ActionResult<List<CustomerDto>>> Get()
        {
            var result = (await _customerService.SearchAsync())
                .Select(o=>new CustomerDto(o))
                .ToList();

            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Post(NewCustomerDto form)
        {
            var producer = _NewCustomerProducer;

            var message = new MessageTypes.LogMessage
            {
                IP = "192.168.0.1",
                Message = "a test message 2",
                Severity = MessageTypes.LogLevel.Info,
                Tags = new Dictionary<string, string> { { "location", "CA" } }
            };

            var dr = await producer.ProduceAsync("newCustomerTopic", new Message<string, MessageTypes.LogMessage> { Key = Guid.NewGuid().ToString(), Value = message });

            producer.Flush();

            Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");

            return Ok(new CustomerDto(new Models.Customer(Guid.NewGuid()) { FirstName = form.FirstName, LastName = form.LastName }));
        }
    }
}