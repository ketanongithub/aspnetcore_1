using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    public class TenantManager : DomainService, ITenantManager
    {
        private readonly IRepository<Tenant, int> _tenantRepository;
        private readonly IRepository<Edition, int> _editionRepository;

        public TenantManager(IRepository<Tenant, int> tenantRepository,
                             IRepository<Edition, int> editionRepository)
        {
            _tenantRepository = tenantRepository;
            _editionRepository = editionRepository;
        }

        public async Task<Tenant> CreateAsync(string name, int editionId)
        {
            var edition = await _editionRepository.GetAsync(editionId);

            var tenant = new Tenant
            {
                TenancyName = name,
                EditionId = editionId,
                CreationTime = DateTime.UtcNow,
                IsActive = true
            };

            await _tenantRepository.InsertAsync(tenant);
            return tenant;
        }

        public async Task<List<Tenant>> GetAllAsync()
        {
            return await _tenantRepository.GetAllListAsync();
        }

        public async Task AssignEditionAsync(int tenantId, int editionId)
        {
            var tenant = await _tenantRepository.GetAsync(tenantId);
            tenant.EditionId = editionId;
            await _tenantRepository.UpdateAsync(tenant);
        }
    }
}
