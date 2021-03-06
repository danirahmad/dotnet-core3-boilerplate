﻿using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;


namespace Moonlay.Confluent.Kafka
{
    public interface IKafkaProducer<TKey, TValue>
    {
        IProducer<TKey, TValue> Producer { get; }

        string TopicName { get; }

        Task Publish(TKey key, TValue value, CancellationToken cancellationToken = default);
    }

    public abstract class KafkaProducer<TKey, TValue> : IKafkaProducer<TKey, TValue>
    {
        private readonly ILogger _logger;

        public KafkaProducer(ILogger logger, ISchemaRegistryClient schemaRegistryClient, ProducerConfig config)
        {
            _logger = logger;
            Producer = new ProducerBuilder<TKey, TValue>(config)
                .SetKeySerializer(new AvroSerializer<TKey>(schemaRegistryClient))
                .SetValueSerializer(new AvroSerializer<TValue>(schemaRegistryClient))
                .Build();
        }

        public IProducer<TKey, TValue> Producer { get; }

        public abstract string TopicName { get; }

        public async Task Publish(TKey key, TValue value, CancellationToken cancellationToken = default)
        {
            var dr = await Producer.ProduceAsync(TopicName, new Message<TKey, TValue> { Key = key, Value = value });

            Producer.Flush();

            _logger.LogInformation($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
        }
    }
}
