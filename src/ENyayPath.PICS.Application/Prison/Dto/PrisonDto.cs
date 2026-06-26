using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Prison.Dto
{
    public class PrisonDto
    {
        public Guid PrisonId { get; set; }

        public string PrisonName { get; set; } = null!;

        public string Address1 { get; set; } = null!;

        public string? Address2 { get; set; }

        public Guid CountryId { get; set; }

        public Guid StateId { get; set; }

        public Guid CityId { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public bool IsActive { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}
