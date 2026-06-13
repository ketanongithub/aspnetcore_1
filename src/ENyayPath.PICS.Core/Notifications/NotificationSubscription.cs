using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Notifications
{
    [Table("SysNotificationSubscriptions")]
    public class NotificationSubscription : Entity<long>, IMayHaveTenant
    {
        public long Id { get; set; }

        public int? TenantId { get; set; }

        public long UserId { get; set; }
        public string NotificationName { get; set; } = default!;
        public string? EntityType { get; set; }
        public string? EntityId { get; set; }

        public DateTime CreationTime { get; set; } = DateTime.UtcNow;
    }
}
