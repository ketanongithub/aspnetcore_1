using AutoMapper;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Application.State.Dtos;
using ENyayPath.PICS.Core.Eny.Common;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.State
{
    [AllowAnonymous]
    public class StateAppService : ApplicationService, IStateAppService
    {
        private readonly IRepository<StateMaster, Guid> _stateRepository;
        private readonly IMapper _mapper;

        public StateAppService(
            IRepository<StateMaster, Guid> stateRepository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _stateRepository = stateRepository;
            _mapper = mapper;
        }

        public async Task<List<StateDto>> GetAllAsync()
        {
            var states = await _stateRepository.GetAllListAsync();
            return _mapper.Map<List<StateDto>>(states);
        }

        public async Task<StateDto> GetAsync(Guid id)
        {
            var state = await _stateRepository.GetAsync(id);
            return _mapper.Map<StateDto>(state);
        }
    }
}
