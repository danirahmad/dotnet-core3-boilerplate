using Moonlay.Confluent.Kafka;
using Moonlay.Core.Models;
using Moonlay.Topics;
using System;

namespace Moonlay.WebApp
{
    internal static class KafkaProducerHelper
    {
        internal static MessageHeader GenMessageHeader(this ISignInService signIn) => new MessageHeader
        {
            AppOrigin = "WebApp",
            CurrentUser = signIn.CurrentUser,
            IsCurrentUserDemo = signIn.Demo,
            Timestamp = DateTime.Now.ToString("s"),
            Token = Guid.NewGuid().ToString()
        };
    }
}
