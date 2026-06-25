using System;
using System.ComponentModel.DataAnnotations;

namespace ENyayPath.PICS.Application.Prisoner.Dtos
{
    public class UpdatePrisonerDto
    {
        [Required]
        public int Id { get; set; }

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

        public string? SonOrDaughterOf { get; set; }
        public string? MotherName { get; set; }
        public string? SpouseName { get; set; }
        public Guid? StateId { get; set; }
        public string? Gender { get; set; }
        public Guid? CountryId { get; set; }
        public Guid? CityId { get; set; }
    }
}
