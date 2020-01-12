using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;
using Moonlay.MCServiceWebApi.Customers;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MCServiceWebApi.Consumers
{
    public interface INewCustomerConsumer : IKafkaConsumer<Topics.MessageHeader, Moonlay.Topics.Customers.NewCustomerTopic> { }

    public class NewCustomerConsumer : INewCustomerConsumer
    {
        private readonly ILogger<NewCustomerConsumer> _logger;

        public IConsumer<Topics.MessageHeader, Moonlay.Topics.Customers.NewCustomerTopic> Consumer { get; }

        private readonly ICustomerService _service;

        public NewCustomerConsumer(ILogger<NewCustomerConsumer> logger, ICustomerService service, ISchemaRegistryClient schemaRegistryClient, ConsumerConfig config)
        {
            _logger = logger;

            // register the consumer
            this.Consumer = new ConsumerBuilder<Topics.MessageHeader, Moonlay.Topics.Customers.NewCustomerTopic>(config)
                .SetKeyDeserializer(new AvroDeserializer<Topics.MessageHeader>(schemaRegistryClient).AsSyncOverAsync())
                .SetValueDeserializer(new AvroDeserializer<Moonlay.Topics.Customers.NewCustomerTopic>(schemaRegistryClient).AsSyncOverAsync())
                .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                .Build();

            _service = service;
        }

        public string TopicName => "new-customer-topic2";

        public async Task Run(CancellationToken cancellationToken = default)
        {
            try
            {
                Consumer.Subscribe(TopicName);

                while (!cancellationToken.IsCancellationRequested)
                {
                    try
                    {
                        var cr = Consumer.Consume(cancellationToken);

                        await _service.NewCustomerAsync(cr.Value.FirstName, cr.Value.LastName, ety =>
                        {
                            ety.CreatedBy = cr.Key.CurrentUser;
                            ety.Tested = cr.Key.IsCurrentUserDemo;
                            ety.UpdatedBy = cr.Key.CurrentUser;
                        });

                        _logger.LogInformation($"Consumed message '{Newtonsoft.Json.JsonConvert.SerializeObject(cr.Message.Key)}' '{Newtonsoft.Json.JsonConvert.SerializeObject(cr.Message.Value)}' at: '{cr.TopicPartitionOffset}'.");

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
    }
}
