using DeliveryApp.Application.DTOs.Messaging;
using DeliveryApp.Domain.Interfaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DeliveryApp.CreatedMotorcycleEventConsumer
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMongoRepository<CreatedMotorcycleEventMessage> _mongoRepository;

        public Worker(ILogger<Worker> logger, IMongoRepository<CreatedMotorcycleEventMessage> mongoRepository)
        {
            _logger = logger;
            _mongoRepository = mongoRepository;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                var factory = new ConnectionFactory() {
                    HostName = "localhost",
                    Port = 5672, // Porta padrão do RabbitMQ
                    UserName = "guest",
                    Password = "guest"
                };

                using var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += async (model, ea)  =>
                {
                    var message = Encoding.UTF8.GetString(ea.Body.ToArray());
                    // Salva no MongoDB aqui
                    var entity = JsonSerializer.Deserialize<CreatedMotorcycleEventMessage>(message);

                    if (entity == null)
                    {
                        _logger.LogError("Failed to deserialize message: {message}", message);
                        return;
                    }

                    if (entity.Year == 2024)
                        await _mongoRepository.CreateAsync(entity);
                };

                channel.BasicConsume("events", true, consumer);
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
