using System;

namespace ENyayPath.PICS.Application.Prisoner.Dtos
{
    public class PrisonerDto
    {
        public int Id { get; set; }
        public Guid PrisonerId { get; set; }
        public Guid PrisonId { get; set; }
        public string PrisonerBatchNo { get; set; } = null!;
        public string PrisonerName { get; set; } = null!;
        public DateTime Dob { get; set; }
        public string? PrisonerStatus { get; set; }
        public int? AllowedMinutesPerWeek { get; set; }
        public bool? IsAudioCallEnabled { get; set; }
        public bool? IsVideoCallEnabled { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime? LastModificationTime { get; set; }

        public string? SonDaughterOf { get; set; }
        public string? MotherName { get; set; }
        public string? SpouseName { get; set; }
        public Guid? StateId { get; set; }
        public string? Gender { get; set; }

        public Guid? CountryId { get; set; }

    }
}
