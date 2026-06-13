using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Text.Json.Serialization;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("EnyRegions")]
    public class Region : FullAuditedEntity<Guid>, IMayHaveTenant
    {
        public Region()
        {

        }

        public int? TenantId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [StringLength(255, ErrorMessage = "Maximum length is 255")]
        public string Name { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public Int64 SupervisorId { get; set; }
        [ForeignKey("SupervisorId")]
        [JsonIgnore]
        public virtual User Supervisor { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public double? TransportRate { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public string Description { get; set; }
    }
}
