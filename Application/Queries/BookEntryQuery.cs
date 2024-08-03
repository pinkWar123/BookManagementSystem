using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class BookEntryQuery : QueryObject
    {
        public int? Id { get; set; }
        public DateOnly? EntryDate { get; set; }    
    }
}
