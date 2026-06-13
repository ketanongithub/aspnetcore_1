using ENyayPath.PICS.Application.PrisonerBiometricData.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerBiometricData
{
    public interface IPrisonerBiometricDataAppService
    {
        Task<List<PrisonerBiometricDataDto>> GetAllAsync();
        Task<PrisonerBiometricDataDto> GetAsync(Guid id);
    }
}
