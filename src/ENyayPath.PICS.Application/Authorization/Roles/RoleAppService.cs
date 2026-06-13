using AutoMapper;
using ENyayPath.PICS.Application.Authorization.Roles.Dtos;
using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Core.Authorization.Roles;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using ENyayPath.PICS.Core.Helpers.Pagination;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using ENyayPath.PICS.Core.Authorization.Permissions;

namespace ENyayPath.PICS.Application.Authorization.Roles
{
    /// <summary>
    /// Application service for role management.
    /// Acts as a bridge between the presentation layer (DTOs) and domain layer (entities/managers).
    /// Implements business logic orchestration and data mapping.
    /// </summary>
    [Authorize(policy: PermissionNames.Roles_Read)]
    public class RoleAppService : ApplicationService, IRoleAppService
    {
        private readonly IRoleManager _roleManager;
        private readonly IMapper _mapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public RoleAppService(
            IRoleManager roleManager,
            IMapper mapper,
            IAppSession appSession,
            IUnitOfWorkManager unitOfWorkManager)
            : base(appSession)
        {
            _roleManager = roleManager ?? throw new ArgumentNullException(nameof(roleManager));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            _unitOfWorkManager = unitOfWorkManager ?? throw new ArgumentNullException(nameof(unitOfWorkManager));
        }

        #region Create

        [Authorize(policy: PermissionNames.Roles_Create)]
        public async Task<RoleDto> CreateRoleAsync(CreateRoleInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            using (var uow = _unitOfWorkManager.Begin())
            {
                var role = await _roleManager.CreateAsync(input.Name, input.Description, AppSession.TenantId);
                await uow.CompleteAsync();
                return _mapper.Map<RoleDto>(role);
            }
        }

        #endregion

        #region Read

        public async Task<RoleDto> GetRoleAsync(long id)
        {
            var role = await _roleManager.GetByIdAsync(id);
            return _mapper.Map<RoleDto>(role);
        }

        public async Task<List<RoleDto>> GetAllRolesAsync()
        {
            var roles = await _roleManager.GetAllAsync(AppSession.TenantId);
            return _mapper.Map<List<RoleDto>>(roles);
        }

        public async Task<PagedRoleResultDto> FilterRolesPagedAsync(GetRolesInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var filterCriteria = input.Filter != null
                ? new RoleFilterCriteria { Name = input.Filter.Name }
                : null;

            var request = new CustomPagedAndSortedResultRequestDto<RoleFilterCriteria>
            {
                SkipCount = input.SkipCount,
                MaxResultCount = input.MaxResultCount,
                Sorting = input.Sorting,
                SortBy = input.SortBy,
                Keyword = input.Keyword,
                IsSearchRegEx = input.IsSearchRegEx,
                Filter = filterCriteria
            };

            var pagedResult = await _roleManager.GetPagedAsync(request, AppSession.TenantId);

            return new PagedRoleResultDto
            {
                TotalCount = pagedResult.TotalCount,
                Items = _mapper.Map<List<RoleDto>>(pagedResult.Items)
            };
        }

        #endregion

        #region Update

        public async Task<RoleDto> UpdateRoleAsync(UpdateRoleInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (!long.TryParse(input.Id, out var roleId) || roleId <= 0)
                throw new ArgumentException("Role ID must be a valid positive number.", nameof(input.Id));

            using (var uow = _unitOfWorkManager.Begin())
            {
                var role = await _roleManager.UpdateAsync(roleId, input.Name, input.Description);
                await uow.CompleteAsync();
                return _mapper.Map<RoleDto>(role);
            }
        }

        #endregion

        #region Delete

        public async Task DeleteRoleAsync(long id)
        {
            using (var uow = _unitOfWorkManager.Begin())
            {
                await _roleManager.DeleteAsync(id);
                await uow.CompleteAsync();
            }
        }

        public async Task DeleteRolesAsync(DeleteRolesInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            if (input.Ids == null || !input.Ids.Any())
                throw new ArgumentException("Role IDs cannot be empty.", nameof(input.Ids));

            using (var uow = _unitOfWorkManager.Begin())
            {
                await _roleManager.DeleteMultipleAsync(input.Ids);
                await uow.CompleteAsync();
            }
        }

        #endregion

        #region Search

        public async Task<List<RoleDto>> SearchRolesAsync(SearchRolesInput input)
        {
            if (input == null)
                throw new ArgumentNullException(nameof(input));

            var roles = await _roleManager.SearchAsync(input.Keyword, input.IsRegex, AppSession.TenantId);
            return _mapper.Map<List<RoleDto>>(roles);
        }

        #endregion
    }
}
