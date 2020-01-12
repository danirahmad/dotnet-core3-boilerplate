using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;
using Moonlay.Topics;
using Moonlay.Topics.Customers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.WebApp.Producers
{
    public interface IUpdateCustomerProducer : IKafkaProducer<MessageHeader, UpdateCustomerTopic> { }

    internal class UpdateCustomerProducer : IUpdateCustomerProducer
    {
        private readonly ILogger<NewCustomerProducer> _logger;

        public UpdateCustomerProducer(ILogger<NewCustomerProducer> logger, IServiceProvider sp)
        {
            _logger = logger;
            Producer = new ProducerBuilder<Topics.MessageHeader, UpdateCustomerTopic>(sp.GetRequiredService<ProducerConfig>())
                .SetKeySerializer(new AvroSerializer<Topics.MessageHeader>(sp.GetRequiredService<ISchemaRegistryClient>()))
                .SetValueSerializer(new AvroSerializer<UpdateCustomerTopic>(sp.GetRequiredService<ISchemaRegistryClient>()))
                .Build();
        }

        public IProducer<Topics.MessageHeader, Topics.Customers.UpdateCustomerTopic> Producer { get; }

        public string TopicName => "new-customer-topic2";

        public async Task Publish(Topics.MessageHeader key, UpdateCustomerTopic value, CancellationToken cancellationToken = default)
        {
            var dr = await Producer.ProduceAsync(TopicName, new Message<Topics.MessageHeader, UpdateCustomerTopic> { Key = key, Value = value });

            Producer.Flush();

            _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
}
