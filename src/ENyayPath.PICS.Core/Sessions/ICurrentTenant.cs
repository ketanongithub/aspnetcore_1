using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Sessions
{
    public interface ICurrentTenant
    {
        int? Id { get; }
        string? Name { get; }
    }
}
