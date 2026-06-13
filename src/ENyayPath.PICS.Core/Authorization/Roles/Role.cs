using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Timers;

namespace ENyayPath.PICS.Core.Authorization.Roles
{
    //public class Role<TPrimaryKey, TUser> : IdentityRole<long>, IMayHaveTenant, IFullAudited<TUser> where TUser : IEntity<long>
    public class Role : IdentityRole<long>, IMayHaveTenant, IEntity<long> //, IFullAudited<User>
    {
        public int? TenantId { get; set; }
        public string? Description { get; set; }


        // Audit fields
        /// <summary>
        /// Reference to the creator user of this entity.
        /// </summary>
        [ForeignKey("CreatorUserId")]
        public virtual User CreatorUser { get; set; }
        public long? CreatorUserId { get; set; }
        public DateTime CreationTime { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Reference to the last modifier user of this entity.
        /// </summary>
        [ForeignKey("LastModifierUserId")]
        public virtual User LastModifierUser { get; set; }
        public long? LastModifierUserId { get; set; }
        public DateTime? LastModificationTime { get; set; }

        // Soft delete
        /// <summary>
        /// Is this entity Deleted?
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// Reference to the deleter user of this entity.
        /// </summary>
        [ForeignKey("DeleterUserId")]
        public virtual User DeleterUser { get; set; }

        /// <summary>
        /// Which user deleted this entity?
        /// </summary>
        public virtual long? DeleterUserId { get; set; }

        /// <summary>
        /// Deletion time of this entity.
        /// </summary>
        public virtual DateTime? DeletionTime { get; set; }

        /// <summary>
        /// Checks if this entity is transient (new, not yet persisted to database).
        /// </summary>
        public bool IsTransient()
        {
            return Id == 0;
        }
    }
}
