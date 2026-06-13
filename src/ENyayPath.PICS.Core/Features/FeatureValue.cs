using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Features
{
    [Table("SysFeatureValues")]
    public class FeatureValue : FullAuditedEntity<Int64>, IAggregateRoot
    {
        public int TenantId { get; set; }
        public int FeatureId { get; set; }

        public string Value { get; set; } = default!;
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
        public int? UpdatedByUserId { get; set; }
    }
}
