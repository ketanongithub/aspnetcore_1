using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.OrganizationUnit
{
    [Table("SysOrganizationUnits")]
    public class OrganizationUnit : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long Id { get; set; }

        public int? TenantId { get; set; }

        public long? ParentId { get; set; }
        public string Code { get; set; } = default!;
        public string DisplayName { get; set; } = default!;
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        public ICollection<UserOrganizationUnit> UserLinks { get; set; } = new List<UserOrganizationUnit>();
        public ICollection<OrganizationUnitRole> RoleLinks { get; set; } = new List<OrganizationUnitRole>();
    }
}
