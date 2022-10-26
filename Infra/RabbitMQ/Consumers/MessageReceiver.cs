using Domain.UseCases.Test;
using Infra.MySql;
using Infra.MySql.Models;
using MediatR;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infra.RabbitMQ.Consumers
{
    public class MessageReceiver : IHostedService
    {
        private readonly RabbitMQSettings _settings;
        private readonly IModel _channel;
        private readonly IMediator _mediator;

        public MessageReceiver(RabbitMQSettings settings, IModel channel, IMediator mediator)
        {
            _settings = settings;
            _channel = channel;
            _mediator = mediator;
        }

        private void SendMessage()
        {
            _channel.ExchangeDeclare(_settings.ExchangeName, _settings.ExchangeType);
            var queueName = _channel.QueueDeclare().QueueName;
            _channel.QueueBind(queueName, _settings.HostName, "msg-queue");

            var consumerAsync = new AsyncEventingBasicConsumer(_channel);

            consumerAsync.Received += async (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var messageModel = JsonSerializer.Deserialize<MessageModel>(message);

                var task = Task.Run(() => _mediator.Send(new TestRequest { InValue = messageModel?.Message ?? "" }));
                await task;
            };

            _channel.BasicConsume(queueName, true, consumerAsync);
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            SendMessage();
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _channel.Dispose();
            return Task.CompletedTask;
        }
    }
}
