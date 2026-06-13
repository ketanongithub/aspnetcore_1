using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Features
{
    [Table("SysFeatures")]
    public class Feature : FullAuditedEntity<Int64>, IMayHaveTenant, IAggregateRoot
    {
        public int? TenantId { get; set; }
        public string Name { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public string ValueType { get; set; } = "string"; // string, int, bool, json
        public string? DefaultValue { get; set; }
        public string? Description { get; set; }


        // Foreign key to Edition
        public int EditionId { get; set; }
        public Edition Edition { get; set; } = default!;
    }
}
