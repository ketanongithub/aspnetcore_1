using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("PrisonerContactDetail")]
    public class PrisonerContactDetail : Entity<Guid>
    {
        [Key]
        [Column("PrisonerContactDetailsId")]
        public override Guid Id { get; set; }

        public Guid PrisonerContactPersonId { get; set; }

        public bool IsAudioCall { get; set; } = true;

        [StringLength(5)]
        public string? PhoneNumberPrefix { get; set; }

        [StringLength(10)]
        public string? PhoneNumber { get; set; }

        [StringLength(200)]
        public string? SIMAffedavitURL { get; set; }

        public bool? IsSIMAffedavitUploaded { get; set; } = false;

        public bool? IsSimValidatedSuccessfully { get; set; } = false;

        [StringLength(20)]
        public string? AppId { get; set; }

        [StringLength(50)]
        public string? RegisteredName { get; set; }

        public bool? IsApproved { get; set; } = false;

        public bool? IsAdharCardUploaded { get; set; } = false;

        public bool? IsActive { get; set; } = true;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
