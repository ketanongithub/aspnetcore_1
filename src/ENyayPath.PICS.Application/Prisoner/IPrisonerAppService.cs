using ENyayPath.PICS.Application.Prisoner.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Prisoner
{
    public interface IPrisonerAppService
    {
        Task<PrisonerDto> GetAsync(int id);
        Task<List<PrisonerDto>> GetAllAsync();
        Task<PrisonerDto> CreateAsync(CreatePrisonerDto input);
        Task<PrisonerDto> UpdateAsync(UpdatePrisonerDto input);
        Task DeleteAsync(int id);
    }
}
