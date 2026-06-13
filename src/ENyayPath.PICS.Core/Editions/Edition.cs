using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.Features;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Editions
{
    [Table("SysEditions")]
    public class Edition : FullAuditedEntity<int>, IMayHaveTenant, IAggregateRoot
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string? Description { get; set; }

        // Pricing / subscription
        public decimal? PricePerMonth { get; set; }
        public decimal? PricePerYear { get; set; }

        // Status
        public bool IsActive { get; set; } = true;

        // Navigation property
        public ICollection<Feature> Features { get; set; } = new List<Feature>();
    }
}
