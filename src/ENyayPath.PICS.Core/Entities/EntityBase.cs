using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Entities
{
    public abstract class EntityBase<TKey>
    {
        public TKey Id { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
    }
}
