using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MCService.Producers
{
    internal interface INewCustomerProducer : IKafkaProducer<string, MessageTypes.LogMessage>
    {
    }

    internal class NewCustomerProducer : INewCustomerProducer
    {
        private readonly ILogger<NewCustomerProducer> _logger;

        public NewCustomerProducer(ILogger<NewCustomerProducer> logger, IServiceProvider sp)
        {
            _logger = logger;
            Producer = new ProducerBuilder<string, MessageTypes.LogMessage>(sp.GetRequiredService<ProducerConfig>())
                .SetKeySerializer(new AvroSerializer<string>(sp.GetRequiredService<ISchemaRegistryClient>()))
                .SetValueSerializer(new AvroSerializer<MessageTypes.LogMessage>(sp.GetRequiredService<ISchemaRegistryClient>()))
                .Build(); 
        }

        public IProducer<string, MessageTypes.LogMessage> Producer { get; }

        public string TopicName => "newCustomerTopic";

        public async Task Publish(string key, MessageTypes.LogMessage value, CancellationToken cancellationToken = default)
        {
            var dr = await Producer.ProduceAsync("newCustomerTopic", new Message<string, MessageTypes.LogMessage> { Key = key, Value = value });

            Producer.Flush();

            _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }


}
