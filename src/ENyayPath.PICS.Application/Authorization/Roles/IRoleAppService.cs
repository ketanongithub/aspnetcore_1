using ENyayPath.PICS.Application.Authorization.Roles.Dtos;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Authorization.Roles
{
    /// <summary>
    /// Application service interface for role management operations.
    /// Exposes business operations to the presentation layer via DTOs.
    /// </summary>
    public interface IRoleAppService
    {
        // --- Create ---
        Task<RoleDto> CreateRoleAsync(CreateRoleInput input);

        // --- Read ---
        Task<RoleDto> GetRoleAsync(long id);
        Task<List<RoleDto>> GetAllRolesAsync();
        Task<PagedRoleResultDto> FilterRolesPagedAsync(GetRolesInput input);

        // --- Update ---
        Task<RoleDto> UpdateRoleAsync(UpdateRoleInput input);

        // --- Delete ---
        Task DeleteRoleAsync(long id);
        Task DeleteRolesAsync(DeleteRolesInput input);

        // --- Search ---
        Task<List<RoleDto>> SearchRolesAsync(SearchRolesInput input);
    }
}
