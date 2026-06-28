using ENyayPath.PICS.Application.PrisonerDocument;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ENyayPath.PICS.Web.Controllers
{
    [ApiController]
    [Route("api/prisoner-documents")]
    [Authorize]
    public class PrisonerDocumentController : ControllerBase
    {
        private readonly IPrisonerDocumentAppService _prisonerDocumentAppService;

        public PrisonerDocumentController(IPrisonerDocumentAppService prisonerDocumentAppService)
        {
            _prisonerDocumentAppService = prisonerDocumentAppService;
        }

        [HttpGet("{id}/download")]
        public async Task<IActionResult> Download(Guid id)
        {
            var result = await _prisonerDocumentAppService.DownloadAsync(id);
            return File(result.Content, result.ContentType, result.FileName);
        }
    }
}
