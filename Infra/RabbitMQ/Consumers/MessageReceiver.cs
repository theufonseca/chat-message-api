using Microsoft.Extensions.Hosting;
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

        public MessageReceiver(RabbitMQSettings settings, IModel channel)
        {
            this.settings = settings;
            this.channel = channel;
        }

        private void DoStuff()
        {
            channel.ExchangeDeclare(exchange: settings.ExchangeName, type: settings.ExchangeType);
            channel.QueueBind(queue: settings.QueueName, exchange: settings.ExchangeName, routingKey: "#.message-sent");

            var consumer = new EventingBasicConsumer(channel);

            consumer.Received += (_, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine($"Message : {message}");
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
