using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Moonlay.MCService.KafkaStream.Topics
{
    public class NewCustomerTopic
    {
        public const string TOPIC_NAME = "newCustomerTopic";

        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
