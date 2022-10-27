using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class MessageEntity
    {
        public string MyNick { get; set; }
        public string FriendNick { get; set; }
        public bool Sent { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public MessageStatus Status { get; set; }
    }
}
