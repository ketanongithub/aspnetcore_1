using System;
using System.Collections.Generic;
using System.Text;

namespace ENyayPath.PICS.Core.Repositories
{
    public interface IUnitOfWork
    {
        Task<int> SaveChangesAsync();
    }
}
