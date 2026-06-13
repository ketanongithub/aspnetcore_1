using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Sessions;
using System.Security.Claims;

namespace ENyayPath.PICS.Web.Sessions
{
    public class AppSession : IAppSession
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public IMultiTenancyConfig MultiTenancy { get; }

        //public long? UserId { get; }

        //public int? TenantId { get; }

        public long? ImpersonatorUserId { get; }

        public int? ImpersonatorTenantId { get; }

        public AppSession(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? UserId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                return long.TryParse(claim, out var id) ? id : null;
            }
        }

        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;

        public int? TenantId
        {
            get
            {
                var claim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id");
                return claim != null && int.TryParse(claim.Value, out var id) ? id : null;
            }
        }

        public MultiTenancySides MultiTenancySide => MultiTenancySides.Tenant;

        public string? TenantName => _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_name")?.Value;
    }
}
