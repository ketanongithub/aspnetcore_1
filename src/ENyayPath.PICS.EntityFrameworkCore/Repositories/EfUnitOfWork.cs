using ENyayPath.PICS.Core.Repositories;
using ENyayPath.PICS.EntityFrameworkCore.DbContexts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.EntityFrameworkCore.Repositories
{
    public class EfUnitOfWork : IUnitOfWork
    {
        private readonly PICSDbContext _context;

        public EfUnitOfWork(PICSDbContext context)
        {
            _context = context;
        }

        public Task<int> SaveChangesAsync() => _context.SaveChangesAsync();
    }
}
