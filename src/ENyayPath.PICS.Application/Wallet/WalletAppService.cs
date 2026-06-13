using AutoMapper;
using ENyayPath.PICS.Application.Wallet.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Wallet
{
    [AllowAnonymous]
    public class WalletAppService : ApplicationService, IWalletAppService
    {
        private readonly IRepository<Core.Eny.Prisoner.Wallet, Guid> _repository;
        private readonly IMapper _mapper;

        public WalletAppService(
            IRepository<Core.Eny.Prisoner.Wallet, Guid> repository,
            IMapper mapper,
            IAppSession appSession)
            : base(appSession)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<WalletDto>> GetAllAsync()
        {
            var items = await _repository.GetAllListAsync();
            return _mapper.Map<List<WalletDto>>(items);
        }

        public async Task<WalletDto> GetAsync(Guid id)
        {
            var item = await _repository.GetAsync(id);
            return _mapper.Map<WalletDto>(item);
        }
    }
}
