using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;
using Moonlay.Topics;
using Moonlay.Topics.Customers;

namespace Moonlay.WebApp.Producers
{
    public interface IUpdateCustomerProducer : IKafkaProducer<MessageHeader, UpdateCustomerTopic> { }

    internal class UpdateCustomerProducer : KafkaProducer<MessageHeader, UpdateCustomerTopic>, IUpdateCustomerProducer
    {
        public UpdateCustomerProducer(ILogger<UpdateCustomerProducer> logger, ISchemaRegistryClient schemaRegistryClient, ProducerConfig config) : base(logger, schemaRegistryClient, config) { }

        public override string TopicName => "update-customer-topic";

    }
}
