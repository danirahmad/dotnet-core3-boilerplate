using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
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
}
