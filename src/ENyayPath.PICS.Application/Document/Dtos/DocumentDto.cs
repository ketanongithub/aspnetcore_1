using System;

namespace ENyayPath.PICS.Application.Document.Dtos
{
    public class DocumentDto
    {
        public Guid DocumentId { get; set; }
        public string DocumentName { get; set; } = null!;
        public string? Description { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
