using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.BackgroundJob
{
    [Table("SysBackgroundJobs")]
    public class BackgroundJobInfo : Entity<long>, IMayHaveTenant
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }

        public string JobName { get; set; } = default!;
        public string JobArgs { get; set; } = default!; // JSON serialized arguments

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
        public DateTime? NextTryTime { get; set; }
        public int TryCount { get; set; } = 0;

        public bool IsAbandoned { get; set; } = false;
        public string? LastException { get; set; }
    }
}
