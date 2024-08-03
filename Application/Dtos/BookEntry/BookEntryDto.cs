namespace BookManagementSystem.Application.Dtos.BookEntry
{
    public class BookEntryDto
    {
        public required int Id { get; set; }
        public DateOnly? EntryDate { get; set; }
    }
}