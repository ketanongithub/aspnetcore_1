using System;

namespace ENyayPath.PICS.Application.PrisonerCallRecord.Dtos
{
    public class PrisonerCallRecordDto
    {
        public Guid PrisonerCallRecordId { get; set; }
        public Guid PrisonerContactDetailsId { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public int? Duration { get; set; }
        public int TypeOfCall { get; set; }
        public decimal? CallCost { get; set; }
        public string? RecordingPath { get; set; }
        public string? Remark { get; set; }
        public bool? IsCallTerminatedByAdmin { get; set; }
        public bool? IsMonitored { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
