using Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.ValueObjects
{
    public class Price : ValueObject
    {
        public decimal Value { get; private set; }

        public Price(decimal value)
        {
            if (value < 0)
                throw new DomainException("Price cannot be negative.");
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

    }
}
