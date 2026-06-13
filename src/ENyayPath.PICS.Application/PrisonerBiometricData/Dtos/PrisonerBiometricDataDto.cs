using System;

namespace ENyayPath.PICS.Application.PrisonerBiometricData.Dtos
{
    public class PrisonerBiometricDataDto
    {
        public Guid PrisonerBiometricDataId { get; set; }
        public Guid PrisonerId { get; set; }
        public int AuthenticationType { get; set; }
        public string BiometricStorageUrl { get; set; } = null!;
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
