using ENyayPath.PICS.Application.Document.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Document
{
    public interface IDocumentAppService
    {
        Task<List<DocumentDto>> GetAllAsync();
        Task<DocumentDto> GetAsync(Guid id);
    }
}
