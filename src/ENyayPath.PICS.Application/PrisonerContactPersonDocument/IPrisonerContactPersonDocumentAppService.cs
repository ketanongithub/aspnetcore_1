using ENyayPath.PICS.Application.PrisonerContactPersonDocument.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactPersonDocument
{
    public interface IPrisonerContactPersonDocumentAppService
    {
        Task<List<PrisonerContactPersonDocumentDto>> GetAllAsync();
        Task<PrisonerContactPersonDocumentDto> GetAsync(Guid id);
    }
}
