using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("Country")]
    public class CountryMaster : Entity<Guid>
    {
        [Key]
        [Column("CountryId")]
        public override Guid Id { get; set; }

        [Required]
        [StringLength(3)]
        public string CountryCode { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string CountryName { get; set; } = null!;

        public bool IsSetTopInList { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
