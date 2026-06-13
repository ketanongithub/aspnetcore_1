using ENyayPath.PICS.Application.ContactApprovalProcess.Dtos;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.ContactApprovalProcess
{
    public interface IContactApprovalProcessAppService
    {
        Task<List<ContactApprovalProcessDto>> GetAllAsync();
        Task<ContactApprovalProcessDto> GetAsync(Guid id);
    }
}
