using Confluent.Kafka;
using Confluent.SchemaRegistry;
using Microsoft.Extensions.Logging;
using Moonlay.Confluent.Kafka;

namespace Moonlay.WebApp.Producers
{
    public interface INewDataSetProducer : IKafkaProducer<Topics.MessageHeader, Topics.MDM.DataSets.NewDataSetTopic> { }
    internal class NewDataSetProducer : KafkaProducer<Topics.MessageHeader, Topics.MDM.DataSets.NewDataSetTopic>, INewDataSetProducer
    {
        public NewDataSetProducer(ILogger<NewDataSetProducer> logger, ISchemaRegistryClient schemaRegistryClient, ProducerConfig config) : base(logger, schemaRegistryClient, config)
        {
        }

        public override string TopicName => "mdm-newdataset-topic";
    }
}
