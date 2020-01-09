
using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Confluent.Kafka.SyncOverAsync;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Moonlay.MCService.KafkaStream.Topics;

namespace Moonlay.MCService
{
    public class KafkaConsumersHosted : IHostedService, IDisposable
    {
        private readonly ILogger<KafkaConsumersHosted> _logger;

        public KafkaConsumersHosted(IServiceProvider services,
            ILogger<KafkaConsumersHosted> logger)
        {
            Services = services;
            _logger = logger;
        }

        public IServiceProvider Services { get; }

        public Task StartAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Kafka Consumers Hosted Service is running.");

            Task.Run(async () => await DoWork(stoppingToken));

            return Task.CompletedTask;
        }

        private Task DoWork(CancellationToken stoppingToken)
        {
            using (var scope = Services.CreateScope())
            {

                var consumerConfig = scope.ServiceProvider.GetRequiredService<ConsumerConfig>();

                var schemaRegistryConfig = scope.ServiceProvider.GetRequiredService<SchemaRegistryConfig>();

                //var avroSerializerConfig = new Confluent.SchemaRegistry.Serdes.AvroSerializerConfig
                //{
                //    // optional Avro serializer properties:
                //    BufferBytes = 100,
                //    AutoRegisterSchemas = true
                //};

                var schemaRegistry = scope.ServiceProvider.GetRequiredService<ISchemaRegistryClient>();

                using var c = new ConsumerBuilder<string, MessageTypes.LogMessage>(consumerConfig)
                    .SetKeyDeserializer(new AvroDeserializer<string>(schemaRegistry).AsSyncOverAsync())
                    .SetValueDeserializer(new AvroDeserializer<MessageTypes.LogMessage>(schemaRegistry).AsSyncOverAsync())
                    .SetErrorHandler((_, e) => Console.WriteLine($"Error: {e.Reason}"))
                    .Build();

                c.Subscribe(new[] { NewCustomerTopic.TOPIC_NAME });

                CancellationTokenSource cts = new CancellationTokenSource();
                Console.CancelKeyPress += (_, e) =>
                {
                    e.Cancel = true; // prevent the process from terminating.
                    cts.Cancel();
                };

                try
                {
                    while (!stoppingToken.IsCancellationRequested)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            _logger.LogInformation($"Consumed message '{cr.Message.Key}' '{Newtonsoft.Json.JsonConvert.SerializeObject(cr.Message.Value)}' at: '{cr.TopicPartitionOffset}'.");

                            switch (cr.Topic)
                            {
                                case NewCustomerTopic.TOPIC_NAME: 
                                    // PUT New Customer Handler here
                                    break;
                            }

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
                    c.Close();
                }
            }

            return Task.CompletedTask;
        }

        public async Task StopAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation(
                "Kafka Consumers Hosted Service is stopping.");

            await Task.CompletedTask;
        }

        public void Dispose()
        {
        }
    }

    internal interface IScopedProcessingService
    {
        Task DoWork(CancellationToken stoppingToken);
    }

    internal class ScopedProcessingService : IScopedProcessingService
    {
        private int executionCount = 0;
        private readonly ILogger _logger;

        public ScopedProcessingService(ILogger<ScopedProcessingService> logger)
        {
            _logger = logger;
        }

        public async Task DoWork(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                executionCount++;

                _logger.LogInformation(
                    "Scoped Processing Service is working. Count: {Count}", executionCount);

                await Task.Delay(10000, stoppingToken);
            }
        }
    }
}