namespace DeliveryApp.Application.Services
{
    public interface IMessageBusService
    {
        //void Publish<T>(T message, string queueName) where T : class;
        void Publish(string message);
    }
}
