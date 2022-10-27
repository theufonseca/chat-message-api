using AutoMapper;
using Domain.Entities;
using Domain.UseCases.Messages;
using MediatR;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infra.RabbitMQ.Consumers
{
    public class MessageReceiver : IHostedService
    {
        private readonly RabbitMQSettings settings;
        private readonly IModel channel;
        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public MessageReceiver(RabbitMQSettings settings, IModel channel, IMediator mediator, IMapper mapper)
        {
            this.settings = settings;
            this.channel = channel;
            this.mediator = mediator;
            this.mapper = mapper;
        }

        private void DoStuff()
        {
            channel.ExchangeDeclare(exchange: settings.ExchangeName, type: settings.ExchangeType);
            channel.QueueBind(queue: settings.QueueName, exchange: settings.ExchangeName, routingKey: "#.message-sent");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var messageString = Encoding.UTF8.GetString(body);
                var messageModel = JsonConvert.DeserializeObject<MessageSentModel>(messageString);

                if (messageModel is null)
                    return;

                var message = mapper.Map<MessageEntity>(messageModel);
                var task = Task.Run(() => mediator.Send(new SendMessageRequest { MessageSent = message }));
                task.Wait();
                var result = task.Result;
            };

            channel.BasicConsume(queue: settings.QueueName, autoAck: true, consumer: consumer);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            DoStuff();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            channel.Dispose();
            return Task.CompletedTask;
        }
    }
}
