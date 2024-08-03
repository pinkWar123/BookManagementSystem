using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class BookEntryDetailQuery : QueryObject
    {
        public int? EntryId { get; set; }
        public int? BookId { get; set; }
        public int? Quantity { get; set; }
        
    }
}
