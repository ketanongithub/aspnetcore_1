using System;

namespace ENyayPath.PICS.Application.ContactApprovalProcess.Dtos
{
    public class ContactApprovalProcessDto
    {
        public Guid PrisonalContactApprovalProcessId { get; set; }
        public Guid PrisonerContactPersonDocumentId { get; set; }
        public Guid ApproverId { get; set; }
        public int ApproverLevel { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
