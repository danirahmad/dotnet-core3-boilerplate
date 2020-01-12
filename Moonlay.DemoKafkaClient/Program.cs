using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Confluent.SchemaRegistry.Serdes;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Moonlay.DemoKafkaClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var config = new ProducerConfig() { BootstrapServers = "192.168.99.100:9092" };
            var schemaRegistryConfig = new SchemaRegistryConfig
            {
                Url = "192.168.99.100:8081",
                // Note: you can specify more than one schema registry url using the
                // schema.registry.url property for redundancy (comma separated list). 
                // The property name is not plural to follow the convention set by
                // the Java implementation.
                // optional schema registry client properties:
                RequestTimeoutMs = 5000,
                MaxCachedSchemas = 10
            };

            // If serializers are not specified, default serializers from
            // `Confluent.Kafka.Serializers` will be automatically used where
            // available. Note: by default strings are encoded as UTF8.
            using var schemaRegistry = new CachedSchemaRegistryClient(schemaRegistryConfig);
            using var p = new ProducerBuilder<string, MessageTypes.LogMessage>(config)
                .SetKeySerializer(new AvroSerializer<string>(schemaRegistry))
                .SetValueSerializer(new AvroSerializer<MessageTypes.LogMessage>(schemaRegistry))
                .Build();

            try
            {
                var message = new MessageTypes.LogMessage
                {
                    IP = "192.168.0.1",
                    Message = "a test message 2",
                    Severity = MessageTypes.LogLevel.Info,
                    Tags = new Dictionary<string, string> { { "location", "CA" } }
                };

                var dr = await p.ProduceAsync("newCustomerTopic", new Message<string, MessageTypes.LogMessage> { Key = Guid.NewGuid().ToString(), Value = message });

                p.Flush();

                Console.WriteLine($"Delivered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
            }
            catch (ProduceException<Null, string> e)
            {
                Console.WriteLine($"Delivery failed: {e.Error.Reason}");
            }
        }
    }
}
