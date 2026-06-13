using System;

namespace ENyayPath.PICS.Application.City.Dtos
{
    public class CityDto
    {
        public Guid CityId { get; set; }
        public Guid CountryId { get; set; }
        public Guid StateId { get; set; }
        public string CityName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
