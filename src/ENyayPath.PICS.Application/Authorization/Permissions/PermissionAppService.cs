using AutoMapper;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Authorization.Permissions;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace ENyayPath.PICS.Application.Authorization.Permissions
{
    /// <summary>
    /// Application service layer for permissions.
    /// Wraps PermissionManager with DTO-friendly methods for API/UI usage.
    /// </summary>
    //[Authorize] // Require authentication for all endpoints
    public class PermissionAppService : ApplicationService, IPermissionAppService
    {
        private readonly IPermissionManager _permissionManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public PermissionAppService(
            IPermissionManager permissionManager,
            IMapper mapper,
            IAppSession appSession,
            IUnitOfWorkManager unitOfWorkManager)
            : base(appSession)
        {
            _permissionManager = permissionManager ?? throw new ArgumentNullException(nameof(permissionManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWorkManager = unitOfWorkManager ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
        }

        // --- Catalog ---
        /// <summary>
        /// Returns all available permissions for the current tenant.
        /// Useful for UI checkboxes on role/user assignment screens.
        /// </summary>
        public async Task<List<string>> GetAllAsync(int? tenantId = null)
        {
            var permissions = await _permissionManager.GetAllAsync(tenantId ?? AppSession.TenantId);
            return new List<string>(permissions);
        }

        // --- Role assignment ---
        public async Task GrantToRoleAsync(long roleId, string permissionName)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _permissionManager.GrantToRoleAsync(roleId, permissionName);
                await uow.CompleteAsync();
            }
        }

        public async Task RevokeFromRoleAsync(long roleId, string permissionName)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _permissionManager.RevokeFromRoleAsync(roleId, permissionName);
                await uow.CompleteAsync();
            }
        }

        public async Task<bool> CheckRolePermissionAsync(long roleId, string permissionName)
        {
            return await _permissionManager.CheckRolePermissionAsync(roleId, permissionName);
        }

        public async Task<List<string>> GetRolePermissionsAsync(long roleId)
        {
            var permissions = await _permissionManager.GetRolePermissionsAsync(roleId);
            return new List<string>(permissions);
        }

        // --- User assignment ---
        public async Task GrantToUserAsync(long userId, string permissionName)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _permissionManager.GrantToUserAsync(userId, permissionName);
                await uow.CompleteAsync();
            }
        }

        public async Task RevokeFromUserAsync(long userId, string permissionName)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _permissionManager.RevokeFromUserAsync(userId, permissionName);
                await uow.CompleteAsync();
            }
        }

        public async Task<bool> CheckUserPermissionAsync(long userId, string permissionName)
        {
            return await _permissionManager.CheckUserPermissionAsync(userId, permissionName);
        }

        public async Task<List<string>> GetUserPermissionsAsync(long userId)
        {
            var permissions = await _permissionManager.GetUserPermissionsAsync(userId);
            return new List<string>(permissions);
        }
    }
}