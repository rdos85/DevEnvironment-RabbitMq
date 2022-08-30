using RabbitMQ.Client;
using System.Text;

namespace PocRabbit
{
    public class ProducerWorker : BackgroundService
    {
        public const string QueueName = "test-queue";
        public const string ExchangeName = "test-exchange";

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            PublishMessage(10);

            return Task.CompletedTask;
        }

        private void PublishMessage(int totalMessages)
        {
            Console.WriteLine($"Iniciando Producer da fila [{QueueName}]. Serão criadas [{totalMessages}] mensagens...");

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
                channel.ExchangeDeclare(ExchangeName, ExchangeType.Direct, true, false, null);
                channel.QueueDeclare(QueueName, true, false, false, null);
                channel.QueueBind(QueueName, ExchangeName, "");

                for (int i = 0; i < totalMessages; i++)
                {
                    var message = $"Hello World! [{i}]";
                    var messageBody = Encoding.UTF8.GetBytes(message);

                    channel.BasicPublish(ExchangeName, "", body: messageBody);

                    Console.WriteLine($"Publicada mensagem --> {message}");
                }
            }
        }
    }
}