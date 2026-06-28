using AutoMapper;
using ENyayPath.PICS.Application.Country.Dtos;
using ENyayPath.PICS.Application.Prison.Dto;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Eny;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Prison
{
    [Authorize]
    public class PrisonAppService : ApplicationService, IPrisonAppService
    {
        private readonly IRepository<Core.Eny.Prison.Prison, Guid> _prisonRepository;
        private readonly IMapper _mapper;

        public PrisonAppService(IAppSession appSession,
            IRepository<Core.Eny.Prison.Prison,Guid> repository,
            IMapper mapper
            ) : base(appSession)
        {
            _prisonRepository = repository;
            _mapper = mapper;    
        }
        public async Task<List<PrisonDto>> GetAllAsync()
        {
            var prisons = await _prisonRepository.GetAllListAsync();
            return _mapper.Map<List<PrisonDto>>(prisons);
        }

        public async Task<PrisonDto> GetAsync(Guid id)
        {
            var prison = await _prisonRepository.GetAsync(id);
            return _mapper.Map<PrisonDto>(prison);
        }
    }
}
