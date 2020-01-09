using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Moonlay.MCServiceConsumers.Consumers
{
    internal interface IKafkaConsumer<TKey, TValue>
    {
        Task Run(IServiceScope scope, CancellationToken cancellationToken = default);

        string TopicName { get; }

        IConsumer<TKey, TValue> Consumer { get; }
    }
}
