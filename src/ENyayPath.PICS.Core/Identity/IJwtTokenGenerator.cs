using ENyayPath.PICS.Core.Authorization.Users;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Identity
{
    public interface IJwtTokenGenerator
    {
        string GenerateJwtToken(User user, IEnumerable<string> roles);
    }
}
