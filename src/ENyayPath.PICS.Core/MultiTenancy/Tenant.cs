using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    [Table("SysTenants")]
    public class Tenant : FullAuditedEntity<int>, IAggregateRoot
    {
        // Basic info
        public string TenancyName { get; set; } = default!;
        public string Code { get; set; } = default!; // short unique identifier
        public string? Description { get; set; }

        // Database / infra
        public string? ConnectionString { get; set; }
        public string? DatabaseProvider { get; set; } = "SqlServer"; // SqlServer, PostgreSQL, MySQL

        // Contact / ownership
        public string? ContactEmail { get; set; }
        public string? ContactPhone { get; set; }
        public string? Address { get; set; }

        // Status / lifecycle
        public bool IsActive { get; set; } = true;
        public DateTime? ActivationDate { get; set; }
        public DateTime? DeactivationDate { get; set; }

        // Subscription / plan
        public string? SubscriptionPlan { get; set; } // e.g., Free, Standard, Enterprise
        public DateTime? SubscriptionExpiry { get; set; }

        // Edition link (important!)
        public int EditionId { get; set; }
        public Edition Edition { get; set; } = default!;

        // Branding / customization
        public string? LogoUrl { get; set; }
        public string? Theme { get; set; } // e.g., dark, light, custom CSS

        // Audit / governance
        public long? CreatedByUserId { get; set; }
        public long? LastUpdatedByUserId { get; set; }
    }
}
