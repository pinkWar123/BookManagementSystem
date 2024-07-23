using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookManagementSystem.Data.Repositories
{
    public interface IUnitOfWork
    {
        // IGenericRepository<User> UserRepository { get; }
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}
