using ENyayPath.PICS.Core.Auditing;
using ENyayPath.PICS.Core.Entities;
using ENyayPath.PICS.Core.MultiTenancy;
using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.EntityFrameworkCore.DbContexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ENyayPath.PICS.EntityFrameworkCore.Repositories
{
    public class EfRepository<TEntity, TPrimaryKey> : IRepository<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {
        private readonly PICSDbContext _dbContext;
        private readonly DbSet<TEntity> _dbSet;
        private int? _tenantId;

        public EfRepository(PICSDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _dbSet = _dbContext.Set<TEntity>();
        }

        public int? CurrentTenantId => _tenantId;

        public void SetTenantContext(int tenantId) => _tenantId = tenantId;

        public IQueryable<TEntity> GetAll() => ApplyTenantFilter(_dbSet);

        public IQueryable<TEntity> GetAllReadonly() => ApplyTenantFilter(_dbSet).AsNoTracking();

        public Task<IQueryable<TEntity>> GetAllReadonlyAsync() => Task.FromResult(GetAllReadonly());

        public Task<IQueryable<TEntity>> GetAllAsync() => Task.FromResult(GetAll());

        public IQueryable<TEntity> GetAllIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
        {
            var query = _dbSet.AsQueryable();
            if (propertySelectors != null)
            {
                foreach (var sel in propertySelectors) query = query.Include(sel);
            }
            return ApplyTenantFilter(query);
        }

        public Task<IQueryable<TEntity>> GetAllIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
            => Task.FromResult(GetAllIncluding(propertySelectors));

        public IQueryable<TEntity> GetAllReadonlyIncluding(params Expression<Func<TEntity, object>>[] propertySelectors)
            => GetAllIncluding(propertySelectors).AsNoTracking();

        public Task<IQueryable<TEntity>> GetAllReadonlyIncludingAsync(params Expression<Func<TEntity, object>>[] propertySelectors)
            => Task.FromResult(GetAllReadonlyIncluding(propertySelectors));

        public List<TEntity> GetAllList() => GetAll().ToList();

        public Task<List<TEntity>> GetAllListAsync() => Task.FromResult(GetAllList());

        public List<TEntity> GetAllList(Expression<Func<TEntity, bool>> predicate) => ApplyTenantFilter(_dbSet.Where(predicate)).ToList();

        public Task<List<TEntity>> GetAllListAsync(Expression<Func<TEntity, bool>> predicate) => Task.FromResult(GetAllList(predicate));

        public T Query<T>(Func<IQueryable<TEntity>, T> queryMethod) => queryMethod(ApplyTenantFilter(_dbSet));

        public TEntity Get(TPrimaryKey id)
        {
            var e = FirstOrDefault(id);
            if (e == null) throw new KeyNotFoundException($"{typeof(TEntity).Name} {id} not found");
            return e;
        }

        public Task<TEntity> GetAsync(TPrimaryKey id) => Task.FromResult(Get(id));

        public TEntity Single(Expression<Func<TEntity, bool>> predicate) => ApplyTenantFilter(_dbSet).Single(predicate);

        public Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> predicate) => Task.FromResult(Single(predicate));

        public TEntity FirstOrDefault(TPrimaryKey id) => ApplyTenantFilter(_dbSet).FirstOrDefault(e => e.Id!.Equals(id));

        public Task<TEntity> FirstOrDefaultAsync(TPrimaryKey id) => Task.FromResult(FirstOrDefault(id));

        public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate) => ApplyTenantFilter(_dbSet).FirstOrDefault(predicate);

        public Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate) => Task.FromResult(FirstOrDefault(predicate));

        public TEntity Load(TPrimaryKey id)
        {
            var instance = Activator.CreateInstance<TEntity>();
            instance.Id = id;
            return instance;
        }

        public TEntity Insert(TEntity entity)
        {
            if (_tenantId.HasValue && entity is IMayHaveTenant mh) mh.TenantId = _tenantId.Value;
            if (entity is IHasCreationTime ct) ct.CreationTime = DateTime.UtcNow;
            _dbSet.Add(entity);
            _dbContext.SaveChanges();
            OnEntityInserted?.Invoke(entity);
            return entity;
        }

        public async Task<TEntity> InsertAsync(TEntity entity)
        {
            if (_tenantId.HasValue && entity is IMayHaveTenant mh) mh.TenantId = _tenantId.Value;
            if (entity is IHasCreationTime ct) ct.CreationTime = DateTime.UtcNow;
            await _dbSet.AddAsync(entity);
            await _dbContext.SaveChangesAsync();
            if (OnEntityInserted != null) await OnEntityInserted.Invoke(entity);
            return entity;
        }

        public TPrimaryKey InsertAndGetId(TEntity entity)
        {
            var e = Insert(entity);
            return e.Id;
        }

        public Task<TPrimaryKey> InsertAndGetIdAsync(TEntity entity) => Task.FromResult(InsertAndGetId(entity));

        public TEntity InsertOrUpdate(TEntity entity) => entity.IsTransient() ? Insert(entity) : Update(entity);

        public Task<TEntity> InsertOrUpdateAsync(TEntity entity) => entity.IsTransient() ? InsertAsync(entity) : UpdateAsync(entity);

        public TPrimaryKey InsertOrUpdateAndGetId(TEntity entity)
        {
            var e = InsertOrUpdate(entity);
            return e.Id;
        }

        public Task<TPrimaryKey> InsertOrUpdateAndGetIdAsync(TEntity entity) => Task.FromResult(InsertOrUpdateAndGetId(entity));

        public TEntity Update(TEntity entity)
        {
            if (entity is IHasModificationTime mt) mt.LastModificationTime = DateTime.UtcNow;
            _dbSet.Update(entity);
            _dbContext.SaveChanges();
            OnEntityUpdated?.Invoke(entity);
            return entity;
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            if (entity is IHasModificationTime mt) mt.LastModificationTime = DateTime.UtcNow;
            _dbSet.Update(entity);
            await _dbContext.SaveChangesAsync();
            if (OnEntityUpdated != null) await OnEntityUpdated.Invoke(entity);
            return entity;
        }

        public TEntity Update(TPrimaryKey id, Action<TEntity> updateAction)
        {
            var entity = Get(id);
            updateAction(entity);
            return Update(entity);
        }

        public async Task<TEntity> UpdateAsync(TPrimaryKey id, Func<TEntity, Task> updateAction)
        {
            var entity = await GetAsync(id);
            await updateAction(entity);
            return await UpdateAsync(entity);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
            _dbContext.SaveChanges();
            OnEntityDeleted?.Invoke(entity);
        }

        public async Task DeleteAsync(TEntity entity)
        {
            _dbSet.Remove(entity);
            await _dbContext.SaveChangesAsync();
            if (OnEntityDeleted != null) await OnEntityDeleted.Invoke(entity);
        }

        public void Delete(TPrimaryKey id)
        {
            var e = Get(id);
            Delete(e);
        }

        public async Task DeleteAsync(TPrimaryKey id)
        {
            var e = await GetAsync(id);
            await DeleteAsync(e);
        }

        public void Delete(Expression<Func<TEntity, bool>> predicate)
        {
            var items = _dbSet.Where(predicate).ToList();
            _dbSet.RemoveRange(items);
            _dbContext.SaveChanges();
        }

        public async Task DeleteAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var items = await _dbSet.Where(predicate).ToListAsync();
            _dbSet.RemoveRange(items);
            await _dbContext.SaveChangesAsync();
        }

        public int Count() => ApplyTenantFilter(_dbSet).Count();

        public Task<int> CountAsync() => ApplyTenantFilter(_dbSet).CountAsync();

        public int Count(Expression<Func<TEntity, bool>> predicate) => ApplyTenantFilter(_dbSet.Where(predicate)).Count();

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate) => ApplyTenantFilter(_dbSet.Where(predicate)).CountAsync();

        public long LongCount() => ApplyTenantFilter(_dbSet).LongCount();

        public Task<long> LongCountAsync() => Task.FromResult(LongCount());

        public long LongCount(Expression<Func<TEntity, bool>> predicate) => ApplyTenantFilter(_dbSet.Where(predicate)).LongCount();

        public Task<long> LongCountAsync(Expression<Func<TEntity, bool>> predicate) => Task.FromResult(LongCount(predicate));

        private IQueryable<TEntity> ApplyTenantFilter(IQueryable<TEntity> query)
        {
            if (!_tenantId.HasValue) return query;
            return query.Where(e => EF.Property<int?>(e, "TenantId") == _tenantId);
        }

        // Audit hooks
        public event Func<TEntity, Task>? OnEntityInserted;
        public event Func<TEntity, Task>? OnEntityUpdated;
        public event Func<TEntity, Task>? OnEntityDeleted;
    }
}
