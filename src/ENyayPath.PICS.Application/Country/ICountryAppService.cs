using ENyayPath.PICS.Application.Country.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Country
{
    public interface ICountryAppService
    {
        Task<List<CountryDto>> GetAllAsync();
        Task<CountryDto> GetAsync(Guid id);
    }
}
