using AutoMapper;
using ENyayPath.PICS.Application.Country.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Country
{
    [AllowAnonymous]
    public class CountryAppService : ApplicationService, ICountryAppService
    {
        private readonly IRepository<CountryMaster, Guid> _countryRepository;
        private readonly IMapper _mapper;

        public CountryAppService(
            IRepository<CountryMaster, Guid> countryRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _countryRepository = countryRepository;
            _mapper = mapper;
        }

        public async Task<List<CountryDto>> GetAllAsync()
        {
            var countries = await _countryRepository.GetAllListAsync();
            return _mapper.Map<List<CountryDto>>(countries);
        }

        public async Task<CountryDto> GetAsync(Guid id)
        {
            var country = await _countryRepository.GetAsync(id);
            return _mapper.Map<CountryDto>(country);
        }
    }
}
