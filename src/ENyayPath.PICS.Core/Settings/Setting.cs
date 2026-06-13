using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Settings
{
    [Table("SysSettings")]
    public class Setting : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long Id { get; set; }

        // Tenant scope (optional)
        public int? TenantId { get; set; }

        // User scope (optional, long for IdentityUser PK)
        public long? UserId { get; set; }

        // Setting details
        public string Name { get; set; } = default!;
        public string Value { get; set; } = default!;
        public string ValueType { get; set; } = "string"; // string, int, bool, json

        public string? Description { get; set; }
        public bool IsEncrypted { get; set; } = false;
        public bool IsSystemSetting { get; set; } = false;
    }
}
