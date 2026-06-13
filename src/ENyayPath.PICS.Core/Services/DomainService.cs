using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Services
{
    /// <summary>
    /// Base class for domain services.
    /// Implements transient dependency for DI.
    /// </summary>
    public abstract class DomainService : DomainServiceBase, IDomainService, ITransientDependency
    {
        protected DomainService() : base() { }
    }
}
