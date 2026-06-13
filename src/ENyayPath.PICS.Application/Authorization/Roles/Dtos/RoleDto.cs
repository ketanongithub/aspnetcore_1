using ENyayPath.PICS.Core.Helpers.Pagination;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ENyayPath.PICS.Application.Authorization.Roles.Dtos
{
    public class RoleDto
    {
        public string Id { get; set; } = default!;
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class CreateRoleInput
    {
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }

    public class UpdateRoleInput
    {
        [Required]
        public string Id { get; set; } = default!;
        [Required]
        public string Name { get; set; } = default!;
        public string? Description { get; set; }
    }


    /// <summary>
    /// Input DTO for getting paginated roles with filtering and sorting.
    /// </summary>
    public class GetRolesInput : CustomPagedAndSortedResultRequestDto<RoleFilterInput>
    {
    }

    /// <summary>
    /// Filter criteria DTO for role searches.
    /// </summary>
    public class RoleFilterInput
    {
        //public DateTime? CreatedFrom { get; set; }
        //public DateTime? CreatedTo { get; set; }
        //public long? CreatorUserId { get; set; }
        public string Name { get; set; }
    }

    /// <summary>
    /// Output DTO for paginated role results.
    /// </summary>
    public class PagedRoleResultDto
    {
        public int TotalCount { get; set; }
        public List<RoleDto> Items { get; set; } = new List<RoleDto>();
    }

    /// <summary>
    /// Input DTO for deleting multiple roles.
    /// </summary>
    public class DeleteRolesInput
    {
        public List<long> Ids { get; set; } = new List<long>();
    }

    /// <summary>
    /// Input DTO for searching roles.
    /// </summary>
    public class SearchRolesInput
    {
        public string Keyword { get; set; }
        public bool IsRegex { get; set; }
    }
}
