using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Infrastructure.Data.Context;
using MongoDB.Driver;

namespace Catalog.Infrastructure.Data.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly IMongoCollection<Product> _products;

        public ProductRepository(MongoDbContext context)
            : base(context, nameof(context.Products))
        {
            _products = context.Products;
        }

        public async Task<Product?> GetBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Eq("Sku.Value", sku);
            return await _products
                .Find(filter)
                .FirstOrDefaultAsync(cancellationToken);
        }

        public async Task<bool> ExistsBySkuAsync(string sku, CancellationToken cancellationToken = default)
        {
            var filter = Builders<Product>.Filter.Eq("Sku.Value", sku);
            return await _products
                .Find(filter)
                .AnyAsync(cancellationToken);
        }
    }
}
