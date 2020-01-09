using Confluent.Kafka;

namespace Moonlay.MCService.KafkaStream
{
    internal interface INewCustomerProducer : IProducer<string, MessageTypes.LogMessage> { }
}