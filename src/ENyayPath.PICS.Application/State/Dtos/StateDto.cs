using System;

namespace ENyayPath.PICS.Application.State.Dtos
{
    public class StateDto
    {
        public Guid StateId { get; set; }
        public Guid CountryId { get; set; }
        public string StateCode { get; set; } = null!;
        public string StateName { get; set; } = null!;
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
