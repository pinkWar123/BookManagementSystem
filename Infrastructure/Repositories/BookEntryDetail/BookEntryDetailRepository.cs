using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.BookEntryDetail
{
    public class BookEntryDetailRepository : GenericRepository<Domain.Entities.BookEntryDetail>, IBookEntryDetailRepository
    {
        public BookEntryDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
    }
}
