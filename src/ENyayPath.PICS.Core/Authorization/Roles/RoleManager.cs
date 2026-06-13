using AutoMapper;
using ENyayPath.PICS.Core.Helpers;
using ENyayPath.PICS.Core.Helpers.Pagination;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.Core.Services;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Core.Authorization.Roles
{
    /// <summary>
    /// Domain service for managing roles.
    /// Implements comprehensive CRUD operations with advanced filtering, pagination, sorting, and validation.
    /// Follows enterprise design patterns including repository pattern, async/await, and multi-tenancy support.
    /// </summary>
    public class RoleManager : DomainService, IRoleManager
    {
        private readonly IRepository<Role, long> _roleRepository;
        private readonly IAppSession _appSession;

        public RoleManager(IRepository<Role, long> roleRepository, IAppSession appSession)
        {
            _roleRepository = roleRepository ?? throw new ArgumentNullException(nameof(roleRepository));
            _appSession = appSession ?? throw new ArgumentNullException(nameof(appSession));
        }

        #region Create

        public async Task<Role> CreateAsync(string name, string description = null, int? tenantId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be empty.", nameof(name));

            tenantId ??= _appSession.TenantId;

            var existingRole = await GetByNameAsync(name, tenantId);
            if (existingRole != null)
                throw new InvalidOperationException($"Role '{name}' already exists.");

            var role = new Role
            {
                Name = name,
                NormalizedName = name.ToUpperInvariant(),
                Description = description,
                TenantId = tenantId,
                CreationTime = DateTime.UtcNow,
                CreatorUserId = _appSession.UserId,
                IsDeleted = false,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            await _roleRepository.InsertAsync(role);
            //await CurrentUnitOfWork.SaveChangesAsync();

            return role;
        }

        #endregion

        #region Read

        public async Task<Role> GetByIdAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentException("Role ID must be greater than 0.", nameof(id));

            var role = await _roleRepository.FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);
            if (role == null)
                throw new KeyNotFoundException($"Role with ID {id} not found.");

            if (role.TenantId != _appSession.TenantId)
                throw new UnauthorizedAccessException("Cross-tenant access denied.");

            return role;
        }

        public async Task<Role> GetByNameAsync(string name, int? tenantId = null)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Role name cannot be empty.", nameof(name));

            tenantId ??= _appSession.TenantId;
            var normalizedName = name.ToUpperInvariant();

            return await _roleRepository.FirstOrDefaultAsync(r =>
                r.NormalizedName == normalizedName &&
                !r.IsDeleted &&
                (tenantId == null || r.TenantId == tenantId));
        }

        public async Task<List<Role>> GetAllAsync(int? tenantId = null)
        {
            tenantId ??= _appSession.TenantId;
            return await _roleRepository.GetAllListAsync(r =>
                !r.IsDeleted && (tenantId == null || r.TenantId == tenantId));
        }

        public async Task<PagedRoleResult> GetPagedAsync(CustomPagedAndSortedResultRequestDto<RoleFilterCriteria> request, int? tenantId = null)
        {
            tenantId ??= _appSession.TenantId;

            var query = (await _roleRepository.GetAllAsync())
                .Where(r => !r.IsDeleted && (tenantId == null || r.TenantId == tenantId));

            if (request.Filter?.Name != null)
                query = query.Where(r => r.Name.Contains(request.Filter.Name));

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var keyword = request.Keyword.ToLowerInvariant();
                query = query.Where(r => r.Name.ToLower().Contains(keyword) ||
                                         (r.Description ?? "").ToLower().Contains(keyword));
            }

            var totalCount = query.Count();

            var items = query
                .Skip(request.SkipCount)
                .Take(request.MaxResultCount)
                .ToList();

            return new PagedRoleResult
            {
                TotalCount = totalCount,
                Items = items
            };
        }

        public async Task<List<Role>> GetByPredicateAsync(Expression<Func<Role, bool>> predicate)
        {
            return await _roleRepository.GetAllListAsync(r => !r.IsDeleted && predicate.Compile()(r));
        }

        public async Task<bool> IsRoleNameExistsAsync(string name, long? excludeId = null, int? tenantId = null)
        {
            tenantId ??= _appSession.TenantId;
            var normalizedName = name.ToUpperInvariant();

            var count = await _roleRepository.CountAsync(r =>
                !r.IsDeleted &&
                r.NormalizedName == normalizedName &&
                (tenantId == null || r.TenantId == tenantId) &&
                (!excludeId.HasValue || r.Id != excludeId.Value));

            return count > 0;
        }

        public async Task<int> GetCountAsync(int? tenantId = null)
        {
            tenantId ??= _appSession.TenantId;
            return await _roleRepository.CountAsync(r => !r.IsDeleted && (tenantId == null || r.TenantId == tenantId));
        }

        #endregion

        #region Update

        public async Task<Role> UpdateAsync(long id, string name, string description = null)
        {
            var role = await GetByIdAsync(id);

            if (role.TenantId != _appSession.TenantId)
                throw new UnauthorizedAccessException("Cross-tenant update denied.");

            role.Name = name;
            role.NormalizedName = name.ToUpperInvariant();
            role.Description = description;
            role.LastModificationTime = DateTime.UtcNow;
            role.LastModifierUserId = _appSession.UserId;
            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            await _roleRepository.UpdateAsync(role);
            await CurrentUnitOfWork.SaveChangesAsync();

            return role;
        }

        public async Task<Role> UpdateRoleAsync(Role role)
        {
            if (role == null)
                throw new ArgumentNullException(nameof(role));

            var existing = await GetByIdAsync(role.Id);

            if (existing.TenantId != _appSession.TenantId)
                throw new UnauthorizedAccessException("Cross-tenant update denied.");

            role.LastModificationTime = DateTime.UtcNow;
            role.LastModifierUserId = _appSession.UserId;
            role.ConcurrencyStamp = Guid.NewGuid().ToString();

            await _roleRepository.UpdateAsync(role);
            await CurrentUnitOfWork.SaveChangesAsync();

            return role;
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(long id)
        {
            var role = await GetByIdAsync(id);

            role.IsDeleted = true;
            role.LastModificationTime = DateTime.UtcNow;
            role.LastModifierUserId = _appSession.UserId;

            await _roleRepository.UpdateAsync(role);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task PermanentlyDeleteAsync(long id)
        {
            var role = await GetByIdAsync(id);
            await _roleRepository.DeleteAsync(role);
            await CurrentUnitOfWork.SaveChangesAsync();
        }

        public async Task DeleteMultipleAsync(IEnumerable<long> ids)
        {
            foreach (var id in ids)
            {
                try
                {
                    await DeleteAsync(id);
                }
                catch (KeyNotFoundException)
                {
                    continue;
                }
            }
        }

        #endregion

        #region Batch

        public async Task<List<Role>> BulkCreateAsync(IEnumerable<(string Name, string Description)> roles, int? tenantId = null)
        {
            var createdRoles = new List<Role>();
            tenantId ??= _appSession.TenantId;

            foreach (var (name, description) in roles)
            {
                var role = await CreateAsync(name, description, tenantId);
                createdRoles.Add(role);
            }

            return createdRoles;
        }

        #endregion

        #region Search

        public async Task<List<Role>> SearchAsync(string keyword, bool isRegex = false, int? tenantId = null)
        {
            tenantId ??= _appSession.TenantId;
            var allRoles = await GetAllAsync(tenantId);

            if (string.IsNullOrWhiteSpace(keyword))
                return allRoles;

            if (isRegex)
            {
                var regex = new Regex(keyword, RegexOptions.IgnoreCase);
                return allRoles.Where(r => regex.IsMatch(r.Name) || regex.IsMatch(r.Description ?? "")).ToList();
            }
            else
            {
                var lowerKeyword = keyword.ToLowerInvariant();
                return allRoles.Where(r => r.Name.ToLowerInvariant().Contains(lowerKeyword) ||
                                           (r.Description ?? "").ToLowerInvariant().Contains(lowerKeyword)).ToList();
            }
        }

        #endregion
    }
}
