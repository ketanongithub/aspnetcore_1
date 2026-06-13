using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Settings
{
    public interface ISettingManager
    {
        Task<string> GetSettingAsync(string name, long? userId = null, int? tenantId = null);
        Task SetSettingAsync(string name, string value, long? userId = null, int? tenantId = null);
    }
}
