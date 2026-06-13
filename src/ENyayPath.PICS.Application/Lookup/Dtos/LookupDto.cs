using System;

namespace ENyayPath.PICS.Application.Lookup.Dtos
{
    public class LookupDto
    {
        public int LookupId { get; set; }
        public string LookupType { get; set; } = null!;
        public string LookupCode { get; set; } = null!;
        public string LookupValue { get; set; } = null!;
        public int SortOrder { get; set; }
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
