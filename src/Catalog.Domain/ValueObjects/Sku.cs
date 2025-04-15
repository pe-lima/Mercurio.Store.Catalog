using Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.ValueObjects
{
    public class Sku: ValueObject
    {
        public string Value { get; private set; }
        public Sku(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("SKU cannot be empty or null.");
            if (value.Length > 20)
                throw new DomainException("SKU cannot be longer than 20 characters.");

            Value = value;
        }
        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }
    }
}
