using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;


namespace Moonlay.Confluent.Kafka
{
    public class KafkaProducer : IKafkaProducer
    {
        private readonly ILogger<KafkaProducer> _logger;
        private readonly ISchemaRegistryClient _schemaRegistryClient;
        private readonly ProducerConfig _config;

        public KafkaProducer(ILogger<KafkaProducer> logger, ISchemaRegistryClient schemaRegistryClient, ProducerConfig config)
        {
            _logger = logger;
            _schemaRegistryClient = schemaRegistryClient;
            _config = config;
        }

        public async Task Publish<TKey, TValue>(string topicName, TKey key, TValue value)
        {
            var producer = new ProducerBuilder<TKey, TValue>(_config)
                .SetKeySerializer(new AvroSerializer<TKey>(_schemaRegistryClient))
                .SetValueSerializer(new AvroSerializer<TValue>(_schemaRegistryClient))
                .Build();

            var dr = await producer.ProduceAsync(topicName, new Message<TKey, TValue> { Key = key, Value = value });

            producer.Flush();

            _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }

    
}
