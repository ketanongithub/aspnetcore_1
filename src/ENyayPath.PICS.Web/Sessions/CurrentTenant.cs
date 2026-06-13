using ENyayPath.PICS.Core.Sessions;

namespace ENyayPath.PICS.Web.Sessions
{
    public class CurrentTenant : ICurrentTenant
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentTenant(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public int? Id
        {
            get
            {
                var tenantClaim = _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_id");
                return tenantClaim != null && int.TryParse(tenantClaim.Value, out var id) ? id : null;
            }
        }

        public string? Name => _httpContextAccessor.HttpContext?.User?.FindFirst("tenant_name")?.Value;
    }
}
