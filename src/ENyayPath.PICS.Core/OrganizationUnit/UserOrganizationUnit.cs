using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.OrganizationUnit
{
    [Table("SysUserOrganizationUnits")]
    public class UserOrganizationUnit : FullAuditedEntity<long>, IMayHaveTenant
    {
        public long Id { get; set; }

        public int? TenantId { get; set; }

        public long UserId { get; set; }
        public long OrganizationUnitId { get; set; }
    }
}
