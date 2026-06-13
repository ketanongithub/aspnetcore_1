using System;

namespace ENyayPath.PICS.Application.PrisonerContactPersonDocument.Dtos
{
    public class PrisonerContactPersonDocumentDto
    {
        public Guid PrisonerContactPersonDocumentId { get; set; }
        public Guid PrisonerContactPersonId { get; set; }
        public Guid DocumentId { get; set; }
        public string? DocumentUploadLink { get; set; }
        public bool? IsValidDocument { get; set; }
        public bool? IsActive { get; set; }
        public Guid CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}
