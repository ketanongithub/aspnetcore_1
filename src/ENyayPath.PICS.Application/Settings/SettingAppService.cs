using ENyayPath.PICS.Application.DTOs;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Sessions;
using ENyayPath.PICS.Core.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Settings
{
    [Authorize(policy: PermissionNames.Settings_Read)]
    public class SettingAppService : ApplicationService, ISettingsAppService
    {
        private readonly ISettingManager _settingsManager;

        public SettingAppService(ISettingManager settingsManager, IAppSession appSession)
            : base(appSession)
        {
            _settingsManager = settingsManager;
        }

        public async Task<SettingDto> GetAsync(string name, long? userId = null, int? tenantId = null)
        {
            var value = await _settingsManager.GetSettingAsync(name, userId, tenantId);

            return new SettingDto
            {
                Name = name,
                Value = value,
                UserId = userId,
                TenantId = tenantId
            };
        }

        [Authorize(policy: PermissionNames.Settings_Create)]
        public async Task SetAsync(SettingDto dto, long? userId = null, int? tenantId = null)
        {
            await _settingsManager.SetSettingAsync(dto.Name, dto.Value, userId, tenantId);
        }
    }
}
