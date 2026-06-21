using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.PrisonerDocument.Dtos
{
    public class UploadPrisonerDocumentDto
    {
        public Guid PrisonerId { get; set; }
        public Guid DocumentId { get; set; }
    }
}
