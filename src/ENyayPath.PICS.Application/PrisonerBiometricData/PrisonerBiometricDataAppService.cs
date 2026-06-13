using AutoMapper;
using ENyayPath.PICS.Application.PrisonerBiometricData.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerBiometricData
{
    [AllowAnonymous]
    public class PrisonerBiometricDataAppService : ApplicationService, IPrisonerBiometricDataAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.PrisonerBiometricData, Guid> _repository;
        private readonly IMapper _mapper;

        public PrisonerBiometricDataAppService(
            IRepository<Core.Eny.Prisoner.PrisonerBiometricData, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<PrisonerBiometricDataDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<PrisonerBiometricDataDto>>(items);
        }

        public async Task<PrisonerBiometricDataDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<PrisonerBiometricDataDto>(item);
        }
    }
}
