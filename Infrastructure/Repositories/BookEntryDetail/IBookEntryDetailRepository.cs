using BookManagementSystem.Data.Repositories;
namespace BookManagementSystem.Infrastructure.Repositories.BookEntryDetail
{
    public interface IBookEntryDetailRepository : IGenericRepository<Domain.Entities.BookEntryDetail>
    {
        Task<Domain.Entities.BookEntryDetail?> GetByIdAsync(int EntryID, int BookID);
        Task<Domain.Entities.BookEntryDetail?> UpdateAsync<TUpdateDto>(int EntryID,int BookID, TUpdateDto entity) where TUpdateDto : class;
    }
}
