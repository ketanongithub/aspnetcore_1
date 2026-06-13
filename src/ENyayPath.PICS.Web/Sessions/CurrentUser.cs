using ENyayPath.PICS.Core.Sessions;
using System.Security.Claims;

namespace ENyayPath.PICS.Web.Sessions
{
    public class CurrentUser : ICurrentUser
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUser(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public long? Id => long.TryParse(
            _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier),
            out var id) ? id : null;

        public string? UserName => _httpContextAccessor.HttpContext?.User?.Identity?.Name;
    }
}
