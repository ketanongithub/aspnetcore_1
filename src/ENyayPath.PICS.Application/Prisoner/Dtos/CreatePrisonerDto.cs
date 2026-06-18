using System;
using System.ComponentModel.DataAnnotations;

namespace ENyayPath.PICS.Application.Prisoner.Dtos
{
    public class CreatePrisonerDto
    {
        public Guid PrisonId { get; set; }

        [Required]
        public string PrisonerBatchNo { get; set; } = null!;

        [Required]
        public string PrisonerName { get; set; } = null!;

        [Required]
        public DateTime Dob { get; set; }

        public string? PrisonerStatus { get; set; }

        public int? AllowedMinutesPerWeek { get; set; }

        public bool? IsAudioCallEnabled { get; set; }

        public bool? IsVideoCallEnabled { get; set; }

        public bool? IsActive { get; set; }

        public string? SonDaughterOf { get; set; }
        public string? MotherName { get; set; }
        public string? SpouseName { get; set; }
        public Guid? StateId { get; set; }
        public string? Gender { get; set; }
    }
}
