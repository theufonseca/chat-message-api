using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.RabbitMQ.Consumers
{
    public class MessageSentModel
    {
        public string MyNick { get; set; }
        public string FriendNick { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
    }
}