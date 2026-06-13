using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Localization
{
    [Table("SysLanguageTexts")]
    public class ApplicationLanguageText : FullAuditedEntity<long>, IMayHaveTenant, IAggregateRoot
    {
        public long Id { get; set; }
        public int? TenantId { get; set; }

        public string LanguageName { get; set; } = default!;
        public string Source { get; set; } = default!; // e.g. "UI"
        public string Key { get; set; } = default!;
        public string Value { get; set; } = default!;
    }
}
