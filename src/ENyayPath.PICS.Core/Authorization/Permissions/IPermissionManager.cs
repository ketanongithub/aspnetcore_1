using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Security;
using System.Security.Claims;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Contract for PermissionManager.
    /// Defines CRUD operations, search, and role/user assignment bridging to Identity claims.
    /// </summary>
    public interface IPermissionManager : ITransientDependency
    {
        // --- Catalog ---
        Task<IList<string>> GetAllAsync(int? tenantId = null);

        // --- Role assignment ---
        Task GrantToRoleAsync(long roleId, string permissionName);
        Task RevokeFromRoleAsync(long roleId, string permissionName);
        Task<bool> CheckRolePermissionAsync(long roleId, string permissionName);
        Task<IList<string>> GetRolePermissionsAsync(long roleId);

        // --- User assignment ---
        Task GrantToUserAsync(long userId, string permissionName);
        Task RevokeFromUserAsync(long userId, string permissionName);
        Task<bool> CheckUserPermissionAsync(long userId, string permissionName);
        Task<IList<string>> GetUserPermissionsAsync(long userId);

    }
}
