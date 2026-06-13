using AutoMapper;
using ENyayPath.PICS.Application.Recharge.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Recharge
{
    [AllowAnonymous]
    public class RechargeAppService : ApplicationService, IRechargeAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.Recharge, Guid> _repository;
        private readonly IMapper _mapper;

        public RechargeAppService(
            IRepository<Core.Eny.Prisoner.Recharge, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RechargeDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<RechargeDto>>(items);
        }

        public async Task<RechargeDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<RechargeDto>(item);
        }
    }
}
