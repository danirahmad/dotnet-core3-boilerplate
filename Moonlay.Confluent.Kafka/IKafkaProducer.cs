using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
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

    public interface IKafkaProducer
    {
        Task Publish<TKey, TValue>(string topicName, TKey key, TValue value);
    }

    
}
