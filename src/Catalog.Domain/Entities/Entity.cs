using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.Domain.Entities
{
    public abstract class Entity
    {
        public Guid Id { get; private set; }
        public DateTime CreatedAt { get; private set; }
        public DateTime? UpdatedAt { get; private set; }
        public bool IsActive { get; private set; }
        
        protected Entity()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.UtcNow;
            IsActive = true;
        }

        public void SetUpdate()
        {
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetDelete()
        {
            IsActive = false;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetActive()
        {
            IsActive = true;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
