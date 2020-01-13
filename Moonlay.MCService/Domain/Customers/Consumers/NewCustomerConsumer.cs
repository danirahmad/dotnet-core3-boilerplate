using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;
using Moonlay.MCServiceWebApi.Customers;
using Moonlay.Topics;
using Moonlay.Topics.Customers;
using System.Threading.Tasks;

namespace Moonlay.MCServiceWebApi.Consumers
{
    public interface INewCustomerConsumer : IKafkaConsumer<MessageHeader, NewCustomerTopic> { }

    internal class NewCustomerConsumer : KafkaConsumer<MessageHeader, NewCustomerTopic>, INewCustomerConsumer
    {
        private readonly ICustomerService _service;

        public NewCustomerConsumer(ILogger<NewCustomerConsumer> logger, ISchemaRegistryClient schemaRegistryClient, ConsumerConfig config, ICustomerService service) : base(logger, schemaRegistryClient, config)
        {
            _service = service;
        }

        public override string TopicName => "new-customer-topic2";

        protected override async Task ConsumeMessage(ConsumeResult<MessageHeader, NewCustomerTopic> cr)
        {
            await _service.NewCustomerAsync(cr.Value.FirstName, cr.Value.LastName, ety =>
            {
                ety.CreatedBy = cr.Key.CurrentUser;
                ety.Tested = cr.Key.IsCurrentUserDemo;
                ety.UpdatedBy = cr.Key.CurrentUser;
            });
        }
    }
}
