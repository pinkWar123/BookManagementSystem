using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.Book
{
    public class BookRepository : GenericRepository<Domain.Entities.Book> ,IBookRepository
    {
        public BookRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}