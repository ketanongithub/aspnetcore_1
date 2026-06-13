using ENyayPath.PICS.Application.State.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.State
{
    public interface IStateAppService
    {
        Task<List<StateDto>> GetAllAsync();
        Task<StateDto> GetAsync(Guid id);
    }
}
