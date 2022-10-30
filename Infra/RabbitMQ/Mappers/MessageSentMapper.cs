using AutoMapper;
using Domain.Entities;
using Infra.RabbitMQ.Consumers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.RabbitMQ.Mappers
{
    public class MessageSentMapper : AutoMapper.Profile
    {
        public MessageSentMapper()
        {
            CreateMap<MessageSentModel, MessageEntity>();
        }
    }
}
