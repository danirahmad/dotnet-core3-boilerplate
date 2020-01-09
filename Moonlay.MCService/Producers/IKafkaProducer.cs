using Confluent.Kafka;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MCService.Producers
{
    internal interface IKafkaProducer<TKey, TValue>
    {
        IProducer<TKey, TValue> Producer { get; }

        string TopicName { get; }

        Task Publish(TKey key, TValue value, CancellationToken cancellationToken = default);
    }
}
