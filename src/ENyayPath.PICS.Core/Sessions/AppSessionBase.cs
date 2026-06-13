using ENyayPath.PICS.Core.MultiTenancy;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Sessions
{
    public abstract class AppSessionBase : IAppSession
    {
        public const string SessionOverrideContextKey = "App.Runtime.Session.Override";

        public IMultiTenancyConfig MultiTenancy { get; }

        public abstract long? UserId { get; }

        public abstract int? TenantId { get; }

        public abstract long? ImpersonatorUserId { get; }

        public abstract int? ImpersonatorTenantId { get; }

        public virtual MultiTenancySides MultiTenancySide
        {
            get
            {
                return MultiTenancy.IsEnabled && !TenantId.HasValue
                    ? MultiTenancySides.Host
                    : MultiTenancySides.Tenant;
            }
        }

        //protected SessionOverride OverridedValue => SessionOverrideScopeProvider.GetValue(SessionOverrideContextKey);
        //protected IAmbientScopeProvider<SessionOverride> SessionOverrideScopeProvider { get; }

        //protected AppSessionBase(IMultiTenancyConfig multiTenancy, IAmbientScopeProvider<SessionOverride> sessionOverrideScopeProvider)
        //{
        //    MultiTenancy = multiTenancy;
        //    SessionOverrideScopeProvider = sessionOverrideScopeProvider;
        //}

        public IDisposable Use(int? tenantId, long? userId)
        {
            //return SessionOverrideScopeProvider.BeginScope(SessionOverrideContextKey, new SessionOverride(tenantId, userId));
            return null;
        }
    }
}
