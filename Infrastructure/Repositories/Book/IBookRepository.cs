using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Domain.Entities;

namespace BookManagementSystem.Infrastructure.Repositories.Book
{
    public interface IBookRepository : IGenericRepository<Domain.Entities.Book>
    {
        Task<List<int>> GetAllBookId();
        Task<bool> BookExistsAsync(string? title, string? genre, string? author);
    }
}
