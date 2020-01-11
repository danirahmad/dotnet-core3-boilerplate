using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MCService.Consumers
{
    internal interface INewCustomerConsumer : IKafkaConsumer<string, MessageTypes.LogMessage> { }
    internal class NewCustomerConsumer : INewCustomerConsumer
    {
        private readonly ILogger<NewCustomerConsumer> _logger;

        public IConsumer<string, MessageTypes.LogMessage> Consumer { get; }

        public NewCustomerConsumer(ILogger<NewCustomerConsumer> logger, IServiceProvider sp)
        {
            _logger = logger;

            // register the consumer
            this.Consumer = new ConsumerBuilder<string, MessageTypes.LogMessage>(sp.GetRequiredService<ConsumerConfig>())
                .SetKeyDeserializer(new AvroDeserializer<string>(sp.GetRequiredService<ISchemaRegistryClient>()).AsSyncOverAsync())
                .SetValueDeserializer(new AvroDeserializer<MessageTypes.LogMessage>(sp.GetRequiredService<ISchemaRegistryClient>()).AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();
        }

        public string TopicName => "newCustomerTopic";

        public Task Run(IServiceScope scope, CancellationToken cancellationToken = default)
        {
            try
            {
                Consumer.Subscribe(TopicName);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = Consumer.Consume(cancellationToken);
                        _logger.LogInformation($"Consumed message '{cr.Message.Key}' '{Newtonsoft.Json.JsonConvert.SerializeObject(cr.Message.Value)}' at: '{cr.TopicPartitionOffset}'.");

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

            return Task.CompletedTask;
        }
    }
}
