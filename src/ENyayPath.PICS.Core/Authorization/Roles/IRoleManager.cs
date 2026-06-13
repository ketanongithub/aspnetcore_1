using ENyayPath.PICS.Core.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Core.Authorization.Roles
{
    /// <summary>
    /// Interface for Role management operations.
    /// Provides CRUD operations with advanced filtering, pagination, sorting, and search capabilities.
    /// </summary>
    public interface IRoleManager
    {
        // --- Create ---
        Task<Role> CreateAsync(string name, string description = null, int? tenantId = null);

        // --- Read ---
        Task<Role> GetByIdAsync(long id);
        Task<Role> GetByNameAsync(string name, int? tenantId = null);
        Task<List<Role>> GetAllAsync(int? tenantId = null);
        Task<PagedRoleResult> GetPagedAsync(CustomPagedAndSortedResultRequestDto<RoleFilterCriteria> request, int? tenantId = null);
        Task<List<Role>> GetByPredicateAsync(Expression<Func<Role, bool>> predicate);
        Task<bool> IsRoleNameExistsAsync(string name, long? excludeId = null, int? tenantId = null);
        Task<int> GetCountAsync(int? tenantId = null);

        // --- Update ---
        Task<Role> UpdateAsync(long id, string name, string description = null);
        Task<Role> UpdateRoleAsync(Role role);

        // --- Delete ---
        Task DeleteAsync(long id);
        Task PermanentlyDeleteAsync(long id);
        Task DeleteMultipleAsync(IEnumerable<long> ids);

        #region Batch Operations

        /// <summary>
        /// Bulk creates multiple roles asynchronously.
        /// </summary>
        /// <param name="roles">Collection of role names to create.</param>
        /// <param name="tenantId">Optional tenant ID for multi-tenant scenarios.</param>
        /// <returns>List of created Role entities.</returns>
        Task<List<Role>> BulkCreateAsync(IEnumerable<(string Name, string Description)> roles, int? tenantId = null);

        #endregion

        #region Export/Search Operations

        /// <summary>
        /// Searches roles by keyword with optional regex support.
        /// </summary>
        /// <param name="keyword">Search keyword (name or description).</param>
        /// <param name="isRegex">Whether to treat keyword as a regex pattern.</param>
        /// <param name="tenantId">Optional tenant ID for multi-tenant search.</param>
        /// <returns>List of matching Role entities.</returns>
        Task<List<Role>> SearchAsync(string keyword, bool isRegex = false, int? tenantId = null);

        #endregion
    }

    /// <summary>
    /// Filter criteria for role queries.
    /// </summary>
    public class RoleFilterCriteria
    {
        public bool? IsActive { get; set; }
        public DateTime? CreatedFrom { get; set; }
        public DateTime? CreatedTo { get; set; }
        public long? CreatorUserId { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Paged result for role queries.
    /// </summary>
    public class PagedRoleResult
    {
        public int TotalCount { get; set; }
        public List<Role> Items { get; set; } = new List<Role>();
    }
}
