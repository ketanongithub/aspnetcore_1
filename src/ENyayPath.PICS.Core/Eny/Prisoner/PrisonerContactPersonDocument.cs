using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("PrisonerContactPersonDocument")]
    public class PrisonerContactPersonDocument : Entity<Guid>
    {
        [Key]
        [Column("PrisonerContactPersonDocumentId")]
        public override Guid Id { get; set; }

        public Guid PrisonerContactPersonId { get; set; }

        public Guid DocumentId { get; set; }

        [StringLength(500)]
        public string? DocumentUploadLink { get; set; }

        public bool? IsValidDocument { get; set; } = false;

        public bool? IsActive { get; set; } = true;

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
