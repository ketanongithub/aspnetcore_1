using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Sessions
{
    public class NullAppSession : AppSessionBase
    {
        /// <summary>
        /// Singleton instance.
        /// </summary>
        public static NullAppSession Instance { get; } = new NullAppSession();

        /// <inheritdoc/>
        public override long? UserId => null;

        /// <inheritdoc/>
        public override int? TenantId => null;

        public override MultiTenancySides MultiTenancySide => MultiTenancySides.Tenant;

        public override long? ImpersonatorUserId => null;

        public override int? ImpersonatorTenantId => null;

        //private NullAppSession()
        //    : base(
        //          new MultiTenancyConfig(),
        //          new DataContextAmbientScopeProvider<SessionOverride>(new AsyncLocalAmbientDataContext())
        //    )
        //{

        //}
    }
}
