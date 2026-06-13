using ENyayPath.PICS.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Settings
{
    public interface ISettingsAppService
    {
        Task<SettingDto> GetAsync(string name, long? userId = null, int? tenantId = null);
        Task SetAsync(SettingDto dto, long? userId = null, int? tenantId = null);
    }
}
