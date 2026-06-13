using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("PrisonerBiometricData")]
    public class PrisonerBiometricData : Entity<Guid>
    {
        [Key]
        [Column("PrisonerBiometricDataId")]
        public override Guid Id { get; set; }

        public Guid PrisonerId { get; set; }

        public int AuthenticationType { get; set; }

        [Required]
        [StringLength(200)]
        public string BiometricStorageUrl { get; set; } = null!;

        public bool? IsActive { get; set; } = true;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
