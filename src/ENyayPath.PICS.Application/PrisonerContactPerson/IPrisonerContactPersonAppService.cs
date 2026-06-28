using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactPerson
{
    public interface IPrisonerContactPersonAppService
    {
        Task<List<PrisonerContactPersonDto>> GetAllAsync();
        Task<PrisonerContactPersonDto> GetAsync(Guid id);
        Task<CreatePrisonerContactPersonDto> CreateAsync(CreatePrisonerContactPersonDto input, List<IFormFile> files);
        Task<PrisonerContactPersonAllDto> GetByPrisonerIdAsync(Guid PrisonerID, bool IsAudioCall);
    }
}
