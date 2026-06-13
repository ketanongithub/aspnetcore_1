using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Sessions
{
    public interface ICurrentUser
    {
        long? Id { get; }
        string? UserName { get; }
    }
}
