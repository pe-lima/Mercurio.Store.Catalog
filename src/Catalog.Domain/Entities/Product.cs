using Catalog.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public class Product : Entity
    {
        public string Name { get; private set; }
        public Description Description { get; private set; }
        public Price Price { get; private set; }
        public Sku Sku { get; private set; }
        public Stock Stock { get; private set; }

        protected Product() { }

        public Product(string name, Description description, Price price, Sku sku, Stock stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.");

            Name = name;
            Description = description;
            Price = price;
            Sku = sku;
            Stock = stock;
        }

        public void Update(string name, Description description, Price price, Sku sku, Stock stock)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Product name is required.");

            Name = name;
            Description = description;
            Price = price;
            Sku = sku;
            Stock = stock;
            SetUpdate();
        }

        public void UpdatePrice(Price newPrice)
        {
            Price = newPrice;
            SetUpdate();
        }

        public void IncreaseStock(int quantity)
        {
            Stock = Stock.Increase(quantity);
            SetUpdate();
        }

        public void DecreaseStock(int quantity)
        {
            Stock = Stock.Decrease(quantity);
            SetUpdate();
        }

        public bool HasStock(int requiredQuantity) => Stock.Quantity >= requiredQuantity;
    }
}
