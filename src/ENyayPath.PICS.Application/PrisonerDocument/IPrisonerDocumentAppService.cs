using ENyayPath.PICS.Application.PrisonerDocument.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.PrisonerDocument
{
    public interface IPrisonerDocumentAppService
    {
        Task<PrisonerDocumentDto> UploadAsync(UploadPrisonerDocumentDto input, IFormFile file);
        Task<PrisonerDocumentDto> GetAsync(Guid id);
        Task<List<PrisonerDocumentDto>> GetAllAsync(Guid prisonerId);
    }
}
