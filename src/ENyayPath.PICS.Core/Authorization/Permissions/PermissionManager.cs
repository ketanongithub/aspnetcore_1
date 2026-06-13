using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Authorization.Users;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Text.RegularExpressions;

namespace ENyayPath.PICS.Core.Authorization.Permissions
{
    /// <summary>
    /// Manages permissions using ASP.NET Core Identity claims.
    /// Provides methods to grant, revoke, and query permissions for roles and users.
    /// </summary>
    public class PermissionManager : DomainService, IPermissionManager
    {
        private readonly RoleManager<IdentityRole<long>> _roleManager;
        private readonly UserManager<IdentityUser<long>> _userManager;

        public PermissionManager(
            RoleManager<IdentityRole<long>> roleManager,
            UserManager<IdentityUser<long>> userManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
        }

        // --- Catalog ---
        public Task<IList<string>> GetAllAsync(int? tenantId = null)
        {
            //tenantId ??= _appSession.TenantId;

            // enforce tenant scope: only return permissions valid for this tenant
            // (here we just return the static catalog, but you could filter by tenant if needed)
            var allPermissions = new List<string>
            {
                PermissionNames.Users_Create,
                PermissionNames.Users_Read,
                PermissionNames.Users_Update,
                PermissionNames.Users_Delete,
                PermissionNames.Roles_Create,
                PermissionNames.Roles_Read,
                PermissionNames.Roles_Update,
                PermissionNames.Roles_Delete,
                PermissionNames.Features_Create,
                PermissionNames.Features_Read,
                PermissionNames.Features_Update,
                PermissionNames.Features_Delete,
                PermissionNames.Editions_Create,
                PermissionNames.Editions_Read,
                PermissionNames.Editions_Update,
                PermissionNames.Editions_Delete,
                PermissionNames.OrgUnits_Create,
                PermissionNames.OrgUnits_Read,
                PermissionNames.OrgUnits_Update,
                PermissionNames.OrgUnits_Delete
            };

            return Task.FromResult<IList<string>>(allPermissions);
        }

        // --- Role assignment ---
        public async Task GrantToRoleAsync(long roleId, string permissionName)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) throw new KeyNotFoundException($"Role {roleId} not found.");

            await _roleManager.AddClaimAsync(role, new Claim("Permission", permissionName));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task RevokeFromRoleAsync(long roleId, string permissionName)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) throw new KeyNotFoundException($"Role {roleId} not found.");

            await _roleManager.RemoveClaimAsync(role, new Claim("Permission", permissionName));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CheckRolePermissionAsync(long roleId, string permissionName)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return false;

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.Any(c => c.Type == "Permission" && c.Value == permissionName);
        }

        public async Task<IList<string>> GetRolePermissionsAsync(long roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId.ToString());
            if (role == null) return new List<string>();

            var claims = await _roleManager.GetClaimsAsync(role);
            return claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();
        }

        // --- User assignment ---
        public async Task GrantToUserAsync(long userId, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new KeyNotFoundException($"User {userId} not found.");

            await _userManager.AddClaimAsync(user, new Claim("Permission", permissionName));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task RevokeFromUserAsync(long userId, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) throw new KeyNotFoundException($"User {userId} not found.");

            await _userManager.RemoveClaimAsync(user, new Claim("Permission", permissionName));
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task<bool> CheckUserPermissionAsync(long userId, string permissionName)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return false;

            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Any(c => c.Type == "Permission" && c.Value == permissionName);
        }

        public async Task<IList<string>> GetUserPermissionsAsync(long userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());
            if (user == null) return new List<string>();

            var claims = await _userManager.GetClaimsAsync(user);
            return claims.Where(c => c.Type == "Permission").Select(c => c.Value).ToList();
        }
    }
}
