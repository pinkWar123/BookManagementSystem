using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore.Storage;

namespace BookManagementSystem.Data.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDBContext _context;
        private IDbContextTransaction _transaction;

        // private ITestRepository _testRepo;

        public UnitOfWork(ApplicationDBContext context)
        {
            _context = context;
        }

        // public IGenericRepository<Test> TestRepository
        // => _testRepo ??= new TestRepository(_context);
        public void Dispose()
        {
            _context.Dispose();
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public async Task<IDbContextTransaction> BeginTransactionAsync()
        {
            if (_transaction == null)
            {
                _transaction = await _context.Database.BeginTransactionAsync();
            }
            return _transaction;
        }

        public async Task CommitAsync()
        {
            await _context.SaveChangesAsync();
            if (_transaction != null)
            {
                await _transaction.CommitAsync();
                await _transaction.DisposeAsync(); // Dispose of the transaction after committing
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction != null)
            {
                await _transaction.RollbackAsync();
                await _transaction.DisposeAsync(); // Dispose of the transaction after rolling back
                _transaction = null;
            }
        }
    }
}
