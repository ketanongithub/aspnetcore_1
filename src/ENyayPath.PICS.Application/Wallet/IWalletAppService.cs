using ENyayPath.PICS.Application.Wallet.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Wallet
{
    public interface IWalletAppService
    {
        Task<List<WalletDto>> GetAllAsync();
        Task<WalletDto> GetAsync(Guid id);
    }
}
