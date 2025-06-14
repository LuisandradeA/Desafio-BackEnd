using DeliveryApp.Domain.Interfaces;
using MongoDB.Driver;

namespace DeliveryApp.Infrastructure.Repositories
{
    public class MongoRepository<T> : IMongoRepository<T> where T : class
    {
        private readonly IMongoCollection<T> _collection;

        public MongoRepository(IMongoDatabase database)
        {
            _collection = database.GetCollection<T>("CreatedMotorcycleEvents");
        }

        public async Task CreateAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }
    }

}
