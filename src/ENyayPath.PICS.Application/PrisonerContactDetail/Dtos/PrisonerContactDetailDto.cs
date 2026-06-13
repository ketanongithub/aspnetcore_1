using System;

namespace ENyayPath.PICS.Application.PrisonerContactDetail.Dtos
{
    public class PrisonerContactDetailDto
    {
        public Guid PrisonerContactDetailsId { get; set; }
        public Guid PrisonerContactPersonId { get; set; }
        public bool IsAudioCall { get; set; }
        public string? PhoneNumberPrefix { get; set; }
        public string? PhoneNumber { get; set; }
        public string? SIMAffedavitURL { get; set; }
        public bool? IsSIMAffedavitUploaded { get; set; }
        public bool? IsSimValidatedSuccessfully { get; set; }
        public string? AppId { get; set; }
        public string? RegisteredName { get; set; }
        public bool? IsApproved { get; set; }
        public bool? IsAdharCardUploaded { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
