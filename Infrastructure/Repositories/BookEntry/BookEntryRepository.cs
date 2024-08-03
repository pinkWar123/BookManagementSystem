using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.BookEntry
{
    public class BookEntryRepository : GenericRepository<Domain.Entities.BookEntry>, IBookEntryRepository
    {
        public BookEntryRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
