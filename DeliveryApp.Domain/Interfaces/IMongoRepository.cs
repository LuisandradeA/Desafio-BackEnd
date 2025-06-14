namespace DeliveryApp.Domain.Interfaces
{
    public interface IMongoRepository<T> where T : class
    {
        Task CreateAsync(T entity);
    }
}
