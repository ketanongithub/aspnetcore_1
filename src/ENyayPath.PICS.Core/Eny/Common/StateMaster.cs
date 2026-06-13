using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("State")]
    public class StateMaster : Entity<Guid>
    {
        [Key]
        [Column("StateId")]
        public override Guid Id { get; set; }

        public Guid CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual CountryMaster Country { get; set; } = null!;

        [Required]
        [StringLength(3)]
        public string StateCode { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string StateName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
