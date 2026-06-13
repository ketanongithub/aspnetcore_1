using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Null implementation of IPermissionManager.
    /// Provides safe defaults when no real manager is configured.
    /// </summary>
    public class NullPermissionManager : IPermissionManager
    {
        public static NullPermissionManager Instance { get; } = new NullPermissionManager();

        public NullPermissionManager() { }

        // --- Catalog ---
        public Task<IList<string>> GetAllAsync(int? tenantId = null) => Task.FromResult((IList<string>)new List<string>());

        // --- Role assignment ---
        public Task GrantToRoleAsync(long roleId, string permissionName) => Task.CompletedTask;
        public Task RevokeFromRoleAsync(long roleId, string permissionName) => Task.CompletedTask;
        public Task<bool> CheckRolePermissionAsync(long roleId, string permissionName) => Task.FromResult(false);
        public Task<IList<string>> GetRolePermissionsAsync(long roleId) => Task.FromResult((IList<string>)new List<string>());

        public Task GrantToUserAsync(long userId, string permissionName) => Task.CompletedTask;
        public Task RevokeFromUserAsync(long userId, string permissionName) => Task.CompletedTask;
        public Task<bool> CheckUserPermissionAsync(long userId, string permissionName) => Task.FromResult(false);
        public Task<IList<string>> GetUserPermissionsAsync(long userId) => Task.FromResult((IList<string>)new List<string>());
    }
}
