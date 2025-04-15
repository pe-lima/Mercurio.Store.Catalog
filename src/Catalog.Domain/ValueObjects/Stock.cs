using Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.ValueObjects
{
    public class Stock : ValueObject
    {
        public int Quantity { get; private set; }

        public Stock(int quantity)
        {
            if (quantity < 0)
                throw new DomainException("Stock cannot be negative.");
            Quantity = quantity;
        }

        public Stock Increase(int amount)
        {
            if(amount <= 0)
                throw new DomainException("Amount to increase must be positive.");
            return new Stock(Quantity + amount);
        }

        public Stock Decrease(int amount)
        {
            if (amount <= 0)
                throw new DomainException("Amount to decrease must be positive.");
            if (Quantity - amount < 0)
                throw new DomainException("Stock cannot be negative.");
            return new Stock(Quantity - amount);
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Quantity;
        }

    }
}
