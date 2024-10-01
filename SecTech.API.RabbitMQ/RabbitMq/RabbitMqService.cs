using Microsoft.Extensions.Logging;
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
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        public RabbitMqService(ILogger<RabbitMqService> logger)
        {
            _logger = logger;
            Task.Run(() => TryConnectAsync(_cancellationTokenSource.Token)); // Запуск процесса переподключения асинхронно
        }

        private async Task TryConnectAsync(CancellationToken cancellationToken)
        {
            int retryCount = 0;
            var factory = new ConnectionFactory() { HostName = "localhost" };

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    _connection = factory.CreateConnection();
                    _channel = _connection.CreateModel();
                    _channel.QueueDeclare(queue: _queueName, durable: true, exclusive: false, autoDelete: false, arguments: null);

                    // Подписка на событие разрыва соединения.
                    _connection.ConnectionShutdown += OnConnectionShutdown;

                    _logger.LogInformation("RabbitMqService initialized successful. Queuename: {queue}", _queueName);
                    retryCount = 0; // Сбрасываем счётчик при успешном подключении
                    return;
                }
                catch (Exception ex)
                {
                    retryCount++;
                    var delay = TimeSpan.FromSeconds(Math.Min(30, Math.Pow(2, retryCount))); // Экспоненциальная задержка с максимумом 30 секунд

                    _logger.LogError(ex, "Failed to connect to RabbitMQ. Retrying in {delay} seconds...", delay.TotalSeconds);
                    await Task.Delay(delay, cancellationToken); // Задержка между попытками
                }
            }
        }

        private void OnConnectionShutdown(object sender, ShutdownEventArgs args)
        {
            _logger.LogWarning("RabbitMQ connection was shut down. Reason: {reason}", args.ReplyText);
            Task.Run(() => TryConnectAsync(_cancellationTokenSource.Token));
        }

        public void SendMessage(object message)
        {
            if (_connection == null || !_connection.IsOpen)
            {
                _logger.LogError("Failed to send message: connection to RabbitMQ is not open.");
                return;
            }

            var messageBody = Encoding.UTF8.GetBytes(System.Text.Json.JsonSerializer.Serialize(message));

            var properties = _channel.CreateBasicProperties();
            properties.Persistent = true; // Для обеспечения сохранности сообщений

            _channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: properties, body: messageBody);
        }

        public void Dispose()
        {
            _cancellationTokenSource.Cancel(); // Отменяем попытки подключения при закрытии
            _channel?.Close();
            _connection?.Close();
            _cancellationTokenSource.Dispose();
        }
    }
}