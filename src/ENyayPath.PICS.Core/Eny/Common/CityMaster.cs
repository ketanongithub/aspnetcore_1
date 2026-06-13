using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("City")]
    public class CityMaster : Entity<Guid>
    {
        [Key]
        [Column("CityId")]
        public override Guid Id { get; set; }

        public Guid CountryId { get; set; }

        [ForeignKey("CountryId")]
        public virtual CountryMaster Country { get; set; } = null!;

        public Guid StateId { get; set; }

        [ForeignKey("StateId")]
        public virtual StateMaster State { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CityName { get; set; } = null!;

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
