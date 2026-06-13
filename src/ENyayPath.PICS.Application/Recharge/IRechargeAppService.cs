using ENyayPath.PICS.Application.Recharge.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Recharge
{
    public interface IRechargeAppService
    {
        Task<List<RechargeDto>> GetAllAsync();
        Task<RechargeDto> GetAsync(Guid id);
    }
}
