using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Localization
{
    [Table("SysLanguages")]
    public class ApplicationLanguage : FullAuditedEntity<int>, IMayHaveTenant, IAggregateRoot
    {
        public int Id { get; set; }
        public int? TenantId { get; set; }

        public string Name { get; set; } = default!; // e.g. "en", "fr"
        public string DisplayName { get; set; } = default!;
        public string? Icon { get; set; }
        public bool IsDisabled { get; set; } = false;
    }
}
