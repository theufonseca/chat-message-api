﻿using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Infra.MySql.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.MySql.Services
{
    public class MessageService : IMessageService
    {
        private readonly IServiceScopeFactory serviceScopeFactory;
        private readonly IMapper mapper;

        public MessageService(IServiceScopeFactory serviceScopeFactory, IMapper mapper)
        {
            this.serviceScopeFactory = serviceScopeFactory;
            this.mapper = mapper;
        }

        public async Task Insert(MessageEntity message)
        {
            using var scope = serviceScopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();

            var messageModel = mapper.Map<MessageModel>(message);
            dbContext.Message.Add(messageModel);
            await dbContext.SaveChangesAsync();
        }
    }
}
