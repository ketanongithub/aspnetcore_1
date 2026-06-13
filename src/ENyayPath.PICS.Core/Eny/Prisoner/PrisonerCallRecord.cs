using ENyayPath.PICS.Core.Entities;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ENyayPath.PICS.Core.Eny.Prisoner
{
    [Table("PrisonerCallRecord")]
    public class PrisonerCallRecord : Entity<Guid>
    {
        [Key]
        [Column("PrisonerCallRecordId")]
        public override Guid Id { get; set; }

        public Guid PrisonerContactDetailsId { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int? Duration { get; set; }

        public int TypeOfCall { get; set; }

        [Column(TypeName = "decimal(10,2)")]
        public decimal? CallCost { get; set; }

        [StringLength(500)]
        public string? RecordingPath { get; set; }

        [StringLength(500)]
        public string? Remark { get; set; }

        public bool? IsCallTerminatedByAdmin { get; set; } = false;

        public bool? IsMonitored { get; set; }

        public Guid CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public Guid? ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}
