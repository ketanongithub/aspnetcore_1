using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.PrisonerDocument.Dtos
{
    public class PrisonerDocumentDto
    {
        public Guid PrisonerDocumentId { get; set; }
        public Guid PrisonerId { get; set; }
        public Guid DocumentId { get; set; }
        public string? DocumentUploadLink { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
