using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("PrisonerContactPerson")]
    public class PrisonerContactPerson : Entity<Guid>
    {
        [Key]
        [Column("PrisonerContactPersonId")]
        public override Guid Id { get; set; }

        public Guid PrisonerId { get; set; }

        [Required]
        [StringLength(100)]
        public string ContactPersonName { get; set; } = null!;

        [Required]
        [StringLength(50)]
        public string Relation { get; set; } = null!;

        public bool? IsTopOnCallList { get; set; } = false;

        public bool? IsApproved { get; set; } = false;

        public bool? IsActive { get; set; } = true;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
