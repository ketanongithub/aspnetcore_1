using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Application.Authorization.Permissions
{
    /// <summary>
    /// Contract for PermissionAppService.
    /// </summary>
    public interface IPermissionAppService
    {
        // --- Catalog ---
        Task<List<string>> GetAllAsync(int? tenantId = null);

        // --- Role assignment ---
        Task GrantToRoleAsync(long roleId, string permissionName);
        Task RevokeFromRoleAsync(long roleId, string permissionName);
        Task<bool> CheckRolePermissionAsync(long roleId, string permissionName);
        Task<List<string>> GetRolePermissionsAsync(long roleId);

        // --- User assignment ---
        Task GrantToUserAsync(long userId, string permissionName);
        Task RevokeFromUserAsync(long userId, string permissionName);
        Task<bool> CheckUserPermissionAsync(long userId, string permissionName);
        Task<List<string>> GetUserPermissionsAsync(long userId);
    }
}
