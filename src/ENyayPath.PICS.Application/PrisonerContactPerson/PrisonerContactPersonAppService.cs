using AutoMapper;
using ENyayPath.PICS.Application.PrisonerContactPerson.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactPerson
{
    [AllowAnonymous]
    public class PrisonerContactPersonAppService : ApplicationService, IPrisonerContactPersonAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> _repository;
        private readonly IMapper _mapper;

        public PrisonerContactPersonAppService(
            IRepository<Core.Eny.Prisoner.PrisonerContactPerson, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrisonerContactPersonDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrisonerContactPersonDto>>(items);
        }

        public async Task<PrisonerContactPersonDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<PrisonerContactPersonDto>(item);
        }
    }
}
