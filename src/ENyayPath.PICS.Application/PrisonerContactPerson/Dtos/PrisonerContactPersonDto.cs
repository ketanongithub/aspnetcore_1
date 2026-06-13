using System;

namespace ENyayPath.PICS.Application.PrisonerContactPerson.Dtos
{
    public class PrisonerContactPersonDto
    {
        public Guid PrisonerContactPersonId { get; set; }
        public Guid PrisonerId { get; set; }
        public string ContactPersonName { get; set; } = null!;
        public string Relation { get; set; } = null!;
        public bool? IsTopOnCallList { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
