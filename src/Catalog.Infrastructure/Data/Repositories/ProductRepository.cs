using Catalog.Domain.Entities;
using Catalog.Domain.Interfaces.Repositories;
using Catalog.Infrastructure.Data.Context;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        public async Task<Product?> GetBySkuAsync(string sku)
        {
            var filter = Builders<Product>.Filter.Eq("Sku.Value", sku);
            return await _products.Find(filter).FirstOrDefaultAsync();
        }

        public async Task<bool> ExistsBySkuAsync(string sku)
        {
            var filter = Builders<Product>.Filter.Eq("Sku.Value", sku);
            return await _products.Find(filter).AnyAsync();
        }
    }
}
