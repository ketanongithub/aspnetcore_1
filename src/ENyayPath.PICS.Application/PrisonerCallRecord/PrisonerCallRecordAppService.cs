using AutoMapper;
using ENyayPath.PICS.Application.PrisonerCallRecord.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerCallRecord
{
    [AllowAnonymous]
    public class PrisonerCallRecordAppService : ApplicationService, IPrisonerCallRecordAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerCallRecord, Guid> _repository;
        private readonly IMapper _mapper;

        public PrisonerCallRecordAppService(
            IRepository<Core.Eny.Prisoner.PrisonerCallRecord, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrisonerCallRecordDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrisonerCallRecordDto>>(items);
        }

        public async Task<PrisonerCallRecordDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<PrisonerCallRecordDto>(item);
        }
    }
}
