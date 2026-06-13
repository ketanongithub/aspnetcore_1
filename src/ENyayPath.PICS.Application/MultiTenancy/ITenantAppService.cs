using ENyayPath.PICS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.MultiTenancy
{
    public interface ITenantAppService
    {
        Task<TenantDto> CreateTenantAsync(string name, int editionId);
        Task<List<TenantDto>> GetTenantsAsync();
        Task AssignEditionAsync(int tenantId, int editionId);
    }
}
