using Catalog.Domain.Interfaces.Repositories;
using Catalog.Infrastructure.Data.Context;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Repositories
{
    public class Repository<T> : IRepository<T> where T : class
    {
        protected readonly IMongoCollection<T> _collection;

        public Repository(MongoDbContext context, string collectionName)
        {
            _collection = context.GetType()
                .GetProperty(collectionName)?
                .GetValue(context) as IMongoCollection<T>
                ?? throw new ArgumentException($"Collection {collectionName} not found in context.");
        }

        public async Task AddAsync(T entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<T?> GetByIdAsync(Guid id)
        {
            var filter = Builders<T>.Filter.Eq("Id", id);
            return await _collection.Find(filter).FirstOrDefaultAsync();
        }

        public void Remove(T entity)
        {
            var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity);
            if (idProp is Guid id)
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                _collection.DeleteOne(filter);
            }
        }

        public void Update(T entity)
        {
            var idProp = entity.GetType().GetProperty("Id")?.GetValue(entity);
            if (idProp is Guid id)
            {
                var filter = Builders<T>.Filter.Eq("Id", id);
                _collection.ReplaceOne(filter, entity);
            }
        }
    }
}
