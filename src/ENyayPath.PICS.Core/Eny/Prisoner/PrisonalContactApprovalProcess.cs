using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("PrisonalContactApprovalProcess")]
    public class PrisonalContactApprovalProcess : Entity<Guid>
    {
        [Key]
        [Column("PrisonalContactApprovalProcessId")]
        public override Guid Id { get; set; }

        public Guid PrisonerContactPersonDocumentId { get; set; }

        public Guid ApproverId { get; set; }

        public int ApproverLevel { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
