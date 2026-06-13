using ENyayPath.PICS.Core.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace ENyayPath.PICS.EntityFrameworkCore.DbContexts
{
    /// <summary>
    /// Production-ready UnitOfWorkManager using EF Core transactions.
    /// Ensures atomic operations across DbContext.
    /// </summary>
    public class EfCoreUnitOfWorkManager : IUnitOfWorkManager
    {
        private readonly PICSDbContext _dbContext;

        public IActiveUnitOfWork Current { get; private set; }

        public EfCoreUnitOfWorkManager(PICSDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public IUnitOfWorkCompleteHandle Begin()
        {
            return Begin(new UnitOfWorkOptions());
        }

        public IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options)
        {
            var transaction = options.IsTransactional
                ? _dbContext.Database.BeginTransaction(options.IsolationLevel ?? IsolationLevel.ReadCommitted)
                : null;

            var uow = new EfCoreActiveUnitOfWork(_dbContext, transaction);
            Current = uow;
            return uow;
        }
    }

    internal class EfCoreActiveUnitOfWork : IActiveUnitOfWork, IUnitOfWorkCompleteHandle
    {
        private readonly PICSDbContext _dbContext;
        private readonly IDbContextTransaction _transaction;
        private bool _completed;

        public EfCoreActiveUnitOfWork(PICSDbContext dbContext, IDbContextTransaction transaction)
        {
            _dbContext = dbContext;
            _transaction = transaction;
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }

        public async Task SaveChangesAsync()
        {
            await _dbContext.SaveChangesAsync();
        }

        public void Complete()
        {
            if (_completed) return;

            _dbContext.SaveChanges();
            _transaction?.Commit();
            _completed = true;
        }

        public async Task CompleteAsync()
        {
            if (_completed) return;

            await _dbContext.SaveChangesAsync();
            if (_transaction != null)
                await _transaction.CommitAsync();

            _completed = true;
        }

        public void Dispose()
        {
            if (!_completed)
            {
                _transaction?.Rollback();
            }
            _transaction?.Dispose();
        }
    }
}
