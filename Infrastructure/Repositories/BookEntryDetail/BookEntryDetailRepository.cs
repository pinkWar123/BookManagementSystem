using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;
using BookManagementSystem.Infrastructure.Repositories.Book;
using BookManagementSystem.Infrastructure.Repositories.BookEntryDetail;

namespace BookManagementSystem.Infrastructure.Repositories.BookEntry
{
    public class BookEntryDetailRepository : GenericRepository<Domain.Entities.BookEntryDetail>, IBookEntryDetailRepository
    {
        public BookEntryDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
