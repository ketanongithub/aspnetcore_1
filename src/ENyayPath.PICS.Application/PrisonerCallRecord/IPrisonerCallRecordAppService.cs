using ENyayPath.PICS.Application.PrisonerCallRecord.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.PrisonerCallRecord
{
    public interface IPrisonerCallRecordAppService
    {
        Task<List<PrisonerCallRecordDto>> GetAllAsync();
        Task<PrisonerCallRecordDto> GetAsync(Guid id);
    }
}
