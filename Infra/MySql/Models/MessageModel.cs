using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.MySql.Models
{
    public class MessageModel
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string MyNick { get; set; }
        public string FriendNick { get; set; }
        public bool Sent { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public MessageStatus Status { get; set; }
    }
}
