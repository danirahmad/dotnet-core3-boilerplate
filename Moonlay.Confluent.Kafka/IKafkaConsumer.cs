using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.Confluent.Kafka
{
    public interface IKafkaConsumer<TKey, TValue>
    {
        Task Run(CancellationToken cancellationToken = default);

        string TopicName { get; }

        IConsumer<TKey, TValue> Consumer { get; }
    }

    public abstract class KafkaConsumer<TKey, TValue> : IKafkaConsumer<TKey, TValue>
    {
        public abstract string TopicName { get; }

        private readonly ILogger _logger;

        public IConsumer<TKey, TValue> Consumer { get; }

        public KafkaConsumer(ILogger logger, ISchemaRegistryClient schemaRegistryClient, ConsumerConfig config)
        {
            _logger = logger;

            // register the consumer
            this.Consumer = new ConsumerBuilder<TKey, TValue>(config)
                .SetKeyDeserializer(new AvroDeserializer<TKey>(schemaRegistryClient).AsSyncOverAsync())
                .SetValueDeserializer(new AvroDeserializer<TValue>(schemaRegistryClient).AsSyncOverAsync())
                .SetErrorHandler((_, e) => System.Console.WriteLine($"Error: {e.Reason}"))
                .Build();
        }

        public async Task Run(CancellationToken cancellationToken = default)
        {
            try
            {
                Consumer.Subscribe(TopicName);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var consumeResult = Consumer.Consume(cancellationToken);

                        await ConsumeMessage(consumeResult);

                        _logger.LogInformation($"Consumed message '{Newtonsoft.Json.JsonConvert.SerializeObject(consumeResult.Message.Key)}' '{Newtonsoft.Json.JsonConvert.SerializeObject(consumeResult.Message.Value)}' at: '{consumeResult.TopicPartitionOffset}'.");

                    }
                    catch (ConsumeException e)
                    {
                        _logger.LogError($"Error occured: {e.Error.Reason}");
                    }
                }
            }
            catch (OperationCanceledException)
            {
                // Ensure the consumer leaves the group cleanly and final offsets are committed.
                Consumer.Close();
            }
        }

        protected abstract Task ConsumeMessage(ConsumeResult<TKey, TValue> consumeResult);
    }
}
