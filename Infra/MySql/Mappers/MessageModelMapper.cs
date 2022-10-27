using AutoMapper;
using Domain.Entities;
using Infra.MySql.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.MySql.Mappers
{
    public class MessageModelMapper : Profile
    {
        public MessageModelMapper()
        {
            CreateMap<MessageEntity, MessageModel>();
            CreateMap<MessageModel, MessageEntity>();
        }
    }
}
