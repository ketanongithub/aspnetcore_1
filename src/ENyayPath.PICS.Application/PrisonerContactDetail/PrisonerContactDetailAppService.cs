using AutoMapper;
using ENyayPath.PICS.Application.PrisonerContactDetail.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerContactDetail
{
    [AllowAnonymous]
    public class PrisonerContactDetailAppService : ApplicationService, IPrisonerContactDetailAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> _repository;
        private readonly IMapper _mapper;

        public PrisonerContactDetailAppService(
            IRepository<Core.Eny.Prisoner.PrisonerContactDetail, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrisonerContactDetailDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrisonerContactDetailDto>>(items);
        }

        public async Task<PrisonerContactDetailDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<PrisonerContactDetailDto>(item);
        }
    }
}
