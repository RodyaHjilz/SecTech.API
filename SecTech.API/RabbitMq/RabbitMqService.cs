using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using System.Text;

namespace SecTech.API.RabbitMq
{
    public class RabbitMqService : IDisposable, IRabbitMqService
    {
        private IConnection _connection;
        private IModel _channel;
        private readonly string _queueName = "reports_queue";
        private readonly ILogger<RabbitMqService> _logger;
        public RabbitMqService(ILogger<RabbitMqService> logger)
        {
            _logger = logger;
            InitializeConnection();
        }
        private void InitializeConnection()
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
            _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);
            // Подписка на событие разрыва соединения.
            _connection.ConnectionShutdown += (sender, args) =>
            {
                _logger.LogError("RabbitMqService connection shutdown. Reason: {reason} \n. Trying reconnect", args.ReplyText);
                Reconnect();
            };

            _logger.LogInformation("RabbitMqService initialized successful. Queuename: {queue}", _queueName);
        }
        
        // Переподключение
        private void Reconnect()
        {
            Dispose();
            InitializeConnection();
        }

        public void SendMessage(object message)
        {
            var messageBody = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Для обеспечения сохранности сообщений

            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: properties, body: messageBody);
        }

        public void Dispose()
        {
            _channel?.Close();
            _connection?.Close();
        }
    }

}
