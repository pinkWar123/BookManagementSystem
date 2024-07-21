using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Infrastructure.Repositories.Book;

namespace BookManagementSystem.Infrastructure.Repositories.BookEntry
{
    public class BookEntryRepository : GenericRepository<Domain.Entities.BookEntry>, IBookEntryRepository
    {
        public BookEntryRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
