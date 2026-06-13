using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Core.Settings
{
    public class SettingManager : DomainService, ISettingManager
    {
        private readonly IRepository<Setting, long> _settingRepository;

        public SettingManager(IRepository<Setting, long> settingRepository)
        {
            _settingRepository = settingRepository;
        }

        public async Task<string> GetSettingAsync(string name, long? userId = null, int? tenantId = null)
        {
            // 1. User-level override
            if (userId.HasValue)
            {
                var userSetting = await _settingRepository.FirstOrDefaultAsync(
                    s => s.Name == name && s.UserId == userId);
                if (userSetting != null) return userSetting.Value;
            }

            // 2. Tenant-level override
            if (tenantId.HasValue)
            {
                var tenantSetting = await _settingRepository.FirstOrDefaultAsync(
                    s => s.Name == name && s.TenantId == tenantId && s.UserId == null);
                if (tenantSetting != null) return tenantSetting.Value;
            }

            // 3. System-level fallback
            var systemSetting = await _settingRepository.FirstOrDefaultAsync(
                s => s.Name == name && s.TenantId == null && s.UserId == null);
            if (systemSetting != null) return systemSetting.Value;

            // 4. Default to empty string if not found
            return string.Empty;
        }

        public async Task SetSettingAsync(string name, string value, long? userId = null, int? tenantId = null)
        {
            Setting? setting = null;

            if (userId.HasValue)
            {
                setting = await _settingRepository.FirstOrDefaultAsync(
                    s => s.Name == name && s.UserId == userId);
            }
            else if (tenantId.HasValue)
            {
                setting = await _settingRepository.FirstOrDefaultAsync(
                    s => s.Name == name && s.TenantId == tenantId && s.UserId == null);
            }
            else
            {
                setting = await _settingRepository.FirstOrDefaultAsync(
                    s => s.Name == name && s.TenantId == null && s.UserId == null);
            }

            if (setting == null)
            {
                setting = new Setting
                {
                    Name = name,
                    Value = value,
                    UserId = userId,
                    TenantId = tenantId,
                    ValueType = "string",
                    LastModificationTime = DateTime.UtcNow
                };
                await _settingRepository.InsertAsync(setting);
            }
            else
            {
                setting.Value = value;
                setting.LastModificationTime = DateTime.UtcNow;
                await _settingRepository.UpdateAsync(setting);
            }
        }
    }
}
