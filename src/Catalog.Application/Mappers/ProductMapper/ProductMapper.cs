using Catalog.Application.DTOs.Product;
using Catalog.Domain.Entities;
using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Application.Mappers.ProductMapper
{
    public class ProductMapper : IMapper<Product, ProductDto>
    {
        public Product ToSource(ProductDto target)
        {
            return new Product(
                target.Name,
                new Description(target.Description),
                new Price(target.Price),
                new Sku(target.Sku),
                new Stock(target.Stock)
            );
        }

        public ProductDto ToTarget(Product source)
        {
            return new ProductDto
            {
                Id = source.Id,
                Name = source.Name,
                Description = source.Description.Value,
                Price = source.Price.Value,
                Sku = source.Sku.Value,
                Stock = source.Stock.Quantity,
                IsActive = source.IsActive,
                CreatedAt = source.CreatedAt,
                UpdatedAt = source.UpdatedAt
            };
        }

    }
}
