using ENyayPath.PICS.Application.City.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.City
{
    public interface ICityAppService
    {
        Task<List<CityDto>> GetAllAsync();
        Task<CityDto> GetAsync(Guid id);
    }
}
