using ENyayPath.PICS.Core.Dependency;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ENyayPath.PICS.Core.Services
{
    /// <summary>
    /// Manages unit of work lifecycles.
    /// Ensures transactional consistency across repositories and services.
    /// </summary>
    public interface IUnitOfWorkManager: ITransientDependency
    {
        IActiveUnitOfWork Current { get; }
        IUnitOfWorkCompleteHandle Begin();
        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }

    public interface IActiveUnitOfWork : ITransientDependency
    {
        void SaveChanges();
        Task SaveChangesAsync();
    }

    public interface IUnitOfWorkCompleteHandle : IDisposable, ITransientDependency
    {
        void Complete();
        Task CompleteAsync();
    }

    public class UnitOfWorkOptions
    {
        public bool IsTransactional { get; set; } = true;
        public IsolationLevel? IsolationLevel { get; set; }
    }
}
