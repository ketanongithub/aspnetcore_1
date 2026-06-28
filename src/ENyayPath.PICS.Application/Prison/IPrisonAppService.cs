using ENyayPath.PICS.Application.Prison.Dto;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Prison
{
    public interface IPrisonAppService
    {
        Task<List<PrisonDto>> GetAllAsync();
        Task<PrisonDto> GetAsync(Guid id);
    }
}
