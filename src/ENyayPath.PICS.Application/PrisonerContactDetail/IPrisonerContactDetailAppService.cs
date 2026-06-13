using ENyayPath.PICS.Application.PrisonerContactDetail.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactDetail
{
    public interface IPrisonerContactDetailAppService
    {
        Task<List<PrisonerContactDetailDto>> GetAllAsync();
        Task<PrisonerContactDetailDto> GetAsync(Guid id);
    }
}
