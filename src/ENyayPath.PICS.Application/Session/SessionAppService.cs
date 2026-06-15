using ENyayPath.PICS.Application.Services;
using ENyayPath.PICS.Application.Session.Dtos;
using ENyayPath.PICS.Core.Identity;
using ENyayPath.PICS.Core.Sessions;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace ENyayPath.PICS.Application.Session
{
    [Authorize]
    public class SessionAppService : ApplicationService, ISessionAppService
    {
        private readonly IIdentityManager _identityManager;

        public SessionAppService(IIdentityManager identityManager, IAppSession appSession)
            : base(appSession)
        {
            _identityManager = identityManager;
        }

        public async Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations()
        {
            var output = new GetCurrentLoginInformationsOutput();

            if (AppSession.UserId.HasValue)
            {
                var user = await _identityManager.FindByIdAsync(AppSession.UserId.Value);
                if (user != null)
                {
                    output.User = new UserLoginInfoDto
                    {
                        UserId = user.Id,
                        Username = user.UserName ?? string.Empty,
                        AliasName = null,
                        Email = user.Email ?? string.Empty,
                        IsActive = !user.IsDeleted,
                        CreatedDate = user.CreationTime
                    };
                }
            }

            return output;
        }
    }
}
