using BookManagementSystem.Data;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Infrastructure.Repositories.BookEntryDetail
{
    public class BookEntryDetailRepository : GenericRepository<Domain.Entities.BookEntryDetail>, IBookEntryDetailRepository
    {
        public BookEntryDetailRepository(ApplicationDBContext applicationDbContext) : base(applicationDbContext)
        {
        }
        public async Task<Domain.Entities.BookEntryDetail?> GetByIdAsync(int EntryID, int BookID)
        {
            return await _context.Set<Domain.Entities.BookEntryDetail>().FindAsync(EntryID, BookID);
        }
        public async Task<Domain.Entities.BookEntryDetail?> UpdateAsync<TUpdateDto>(int EntryID, int BookID, TUpdateDto entity) where TUpdateDto : class
        {
            var existingEntity = await GetByIdAsync(EntryID, BookID);
            if (existingEntity == null)
            {
            return null;
            }
            _context.Entry(existingEntity).CurrentValues.SetValues(entity);
            return existingEntity;
        }
    }
}
