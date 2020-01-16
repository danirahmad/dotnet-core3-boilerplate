using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;
using Moonlay.MasterData.WebApi.Customers;
using Moonlay.Topics;
using Moonlay.Topics.Customers;
using System;
using System.Threading.Tasks;

namespace Moonlay.MasterData.WebApi.Consumers
{
    public interface IUpdateCustomerConsumer : IKafkaConsumer<MessageHeader, UpdateCustomerTopic> { }

    internal class UpdateCustomerConsumer : KafkaConsumer<MessageHeader, UpdateCustomerTopic>, IUpdateCustomerConsumer
    {
        public override string TopicName => "update-customer-topic";

        private readonly ICustomerService _service;

        public UpdateCustomerConsumer(ILogger<UpdateCustomerConsumer> logger, ISchemaRegistryClient schemaRegistryClient, ConsumerConfig config, ICustomerService service) : base(logger, schemaRegistryClient, config)
        {
            _service = service;
        }

        protected override async Task ConsumeMessage(ConsumeResult<MessageHeader, UpdateCustomerTopic> consumeResult)
        {
            await _service.UpdateProfileAsync(Guid.Parse(consumeResult.Value.CustomerId), consumeResult.Value.FirstName, consumeResult.Value.LastName, ety =>
            {
                ety.UpdatedBy = consumeResult.Key.CurrentUser;
            });
        }
    }
}
