using Catalog.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.ValueObjects
{
    public class Description : ValueObject
    {
        public string Value { get; private set; }

        public Description(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new DomainException("Description cannot be empty or null.");
            if (value.Length > 500)
                throw new DomainException("Description cannot be longer than 500 characters.");
            if (value.Length < 10)
                throw new DomainException("Description cannot be shorter than 10 characters.");
            Value = value;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            yield return Value;
        }

        public override string ToString() => Value;
    }
}
