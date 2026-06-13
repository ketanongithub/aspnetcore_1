using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Common
{
    [Table("Lookup")]
    public class LookupMaster : Entity<int>
    {
        [Key]
        [Column("LookupId")]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public override int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string LookupType { get; set; } = null!;

        [Required]
        [StringLength(20)]
        public string LookupCode { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string LookupValue { get; set; } = null!;

        public int SortOrder { get; set; }

        [StringLength(200)]
        public string? Description { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    }
}
