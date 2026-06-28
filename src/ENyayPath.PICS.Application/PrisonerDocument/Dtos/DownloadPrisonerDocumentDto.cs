using System.IO;

namespace ENyayPath.PICS.Application.PrisonerDocument.Dtos
{
    public class DownloadPrisonerDocumentDto
    {
        public Stream Content { get; set; } = null!;
        public string FileName { get; set; } = null!;
        public string ContentType { get; set; } = null!;
    }
}
