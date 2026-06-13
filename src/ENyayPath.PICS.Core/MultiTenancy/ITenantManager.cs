using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    public interface ITenantManager
    {
        Task<Tenant> CreateAsync(string name, int editionId);
        Task<List<Tenant>> GetAllAsync();
        Task AssignEditionAsync(int tenantId, int editionId);
    }
}
