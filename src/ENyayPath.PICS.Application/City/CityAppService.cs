using AutoMapper;
using ENyayPath.PICS.Application.City.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.City
{
    [AllowAnonymous]
    public class CityAppService : ApplicationService, ICityAppService
    {
        private readonly IRepository<CityMaster, Guid> _cityRepository;
        private readonly IMapper _mapper;

        public CityAppService(
            IRepository<CityMaster, Guid> cityRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _cityRepository = cityRepository;
            _mapper = mapper;
        }

        public async Task<List<CityDto>> GetAllAsync()
        {
            var cities = await _cityRepository.GetAllListAsync();
            return _mapper.Map<List<CityDto>>(cities);
        }

        public async Task<CityDto> GetAsync(Guid id)
        {
            var city = await _cityRepository.GetAsync(id);
            return _mapper.Map<CityDto>(city);
        }
    }
}
