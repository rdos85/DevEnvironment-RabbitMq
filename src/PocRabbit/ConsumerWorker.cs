using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PocRabbit
{
    public class ConsumerWorker : BackgroundService
    {

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            ConsumeMessage(stoppingToken);
        }

        private void ConsumeMessage(CancellationToken stoppingToken)
        {
            Console.WriteLine($"Iniciando Consumer da fila [{ProducerWorker.QueueName}]... [Pressione Ctrl+C para parar]");

            var factory = new ConnectionFactory()
            {
                HostName = "::1",
                UserName = "admin",
                Password = "admin",
                Port = 5672
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (sender, deliveredMessage) =>
                {
                    var message = Encoding.UTF8.GetString(deliveredMessage.Body.ToArray());
                    Console.WriteLine($"Recebida mensagem --> {message}");
                    channel.BasicAck(deliveredMessage.DeliveryTag, false);
                };

                var x = channel.BasicConsume(ProducerWorker.QueueName, autoAck: false, consumer);

                stoppingToken.WaitHandle.WaitOne();

                Console.WriteLine($"Desligando Consumer da fila [{ProducerWorker.QueueName}].");
            }
        }
    }
}
