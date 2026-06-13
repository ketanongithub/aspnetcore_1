using ENyayPath.PICS.Application.DTOs;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Editions;
using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Sessions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.MultiTenancy
{
    public class TenantAppService : ApplicationService, ITenantAppService
    {
        private readonly ITenantManager _tenantManager;

        public TenantAppService(ITenantManager tenantManager, IAppSession appSession)
            : base(appSession)
        {
            _tenantManager = tenantManager;
        }

        public async Task<TenantDto> CreateTenantAsync(string name, int editionId)
        {
            var tenant = await _tenantManager.CreateAsync(name, editionId);
            return new TenantDto
            {
                Id = tenant.Id,
                TenancyName = tenant.TenancyName,
                EditionName = tenant.Edition?.Name ?? string.Empty
            };
        }

        public async Task<List<TenantDto>> GetTenantsAsync()
        {
            var tenants = await _tenantManager.GetAllAsync();
            return tenants.Select(t => new TenantDto
            {
                Id = t.Id,
                TenancyName = t.TenancyName,
                EditionName = t.Edition?.Name ?? string.Empty
            }).ToList();
        }

        public async Task AssignEditionAsync(int tenantId, int editionId)
        {
            await _tenantManager.AssignEditionAsync(tenantId, editionId);
        }
    }
}
