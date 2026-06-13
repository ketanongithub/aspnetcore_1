using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.MultiTenancy
{
    /// <summary>
    /// Optional tenant association.
    /// </summary>
    public interface IMayHaveTenant
    {
        int? TenantId { get; set; }
    }
}
