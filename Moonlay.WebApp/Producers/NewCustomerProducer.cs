using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;

namespace Moonlay.WebApp.Producers
{
    public interface INewCustomerProducer : IKafkaProducer<Topics.MessageHeader, Topics.Customers.NewCustomerTopic> { }

    internal class NewCustomerProducer : KafkaProducer<Topics.MessageHeader, Topics.Customers.NewCustomerTopic>, INewCustomerProducer
    {
        public NewCustomerProducer(ILogger<NewCustomerProducer> logger, ISchemaRegistryClient schemaRegistryClient, ProducerConfig config) : base(logger, schemaRegistryClient, config)
        {
        }

        public override string TopicName => "new-customer-topic2";
    }
}
