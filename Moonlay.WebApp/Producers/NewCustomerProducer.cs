using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.WebApp.Producers
{
    public interface INewCustomerProducer : IKafkaProducer<Topics.MessageHeader, Topics.Customers.NewCustomerTopic> { }

    internal class NewCustomerProducer : INewCustomerProducer
    {
        private readonly ILogger<NewCustomerProducer> _logger;

        public NewCustomerProducer(ILogger<NewCustomerProducer> logger, IServiceProvider sp)
        {
            _logger = logger;
            Producer = new ProducerBuilder<Topics.MessageHeader, Topics.Customers.NewCustomerTopic>(sp.GetRequiredService<ProducerConfig>())
                .SetKeySerializer(new AvroSerializer<Topics.MessageHeader>(sp.GetRequiredService<ISchemaRegistryClient>()))
                .SetValueSerializer(new AvroSerializer<Topics.Customers.NewCustomerTopic>(sp.GetRequiredService<ISchemaRegistryClient>()))
                .Build();
        }

        public IProducer<Topics.MessageHeader, Topics.Customers.NewCustomerTopic> Producer { get; }

        public string TopicName => "new-customer-topic2";

        public async Task Publish(Topics.MessageHeader key, Topics.Customers.NewCustomerTopic value, CancellationToken cancellationToken = default)
        {
            var dr = await Producer.ProduceAsync(TopicName, new Message<Topics.MessageHeader, Topics.Customers.NewCustomerTopic> { Key = key, Value = value });

            Producer.Flush();

            _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }


}
