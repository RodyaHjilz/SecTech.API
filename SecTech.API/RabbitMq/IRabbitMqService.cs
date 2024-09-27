namespace SecTech.API.RabbitMq
{
    public interface IRabbitMqService
    {
        public void SendMessage(object message);
    }
}