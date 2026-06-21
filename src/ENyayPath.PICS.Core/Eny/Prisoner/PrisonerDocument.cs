using ENyayPath.PICS.Core.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    public class PrisonerDocument : FullAuditedEntity<Guid>
    {
        [Key]
        [Column("PrisonerDocumentId")]
        public override Guid Id { get; set; }

        public Guid PrisonerId { get; set; }

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
