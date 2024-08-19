using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace BookManagementSystem.Infrastructure.Repositories.Book
{
    public class BookRepository : GenericRepository<Domain.Entities.Book>, IBookRepository
    {
        public BookRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<List<int>> GetAllBookId()
        {
            return await _context.Books.Select(b => b.Id).ToListAsync();
        }
        public async Task<bool> BookExistsAsync(string? title, string? genre, string? author)
        {
            return await _context.Books
                .AnyAsync(b => b.Title == title && b.Genre == genre && b.Author == author);
        }
    }
}
