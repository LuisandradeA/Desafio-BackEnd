using DeliveryApp.Application.Services;
using EasyNetQ;
using RabbitMQ.Client;
using System.Text;

namespace DeliveryApp.Services.Services
{
    public class MessageBusService : IMessageBusService
    {
        public void Publish(string message)
        {
            var factory = new ConnectionFactory() { 
                HostName = "localhost",
                Port = 5672, // Porta padrão do RabbitMQ
                UserName = "guest",
                Password = "guest"
            };
            using var connection = factory.CreateConnection();
            using var channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "events",
                durable: true,  // ← Alterar para true
                exclusive: false,
                autoDelete: false,
                arguments: null);

            channel.BasicPublish(
                exchange: "",
                routingKey: "events",
                body: Encoding.UTF8.GetBytes(message)
            );
        }
    }
}
