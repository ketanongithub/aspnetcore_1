using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactPerson
{
    public interface IPrisonerContactPersonAppService
    {
        Task<List<PrisonerContactPersonDto>> GetAllAsync();
        Task<PrisonerContactPersonDto> GetAsync(Guid id);
    }
}
