using System;

namespace ENyayPath.PICS.Application.Country.Dtos
{
    public class CountryDto
    {
        public Guid CountryId { get; set; }
        public string CountryCode { get; set; } = null!;
        public string CountryName { get; set; } = null!;
        public bool IsSetTopInList { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
