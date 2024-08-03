namespace BookManagementSystem.Application.Dtos.BookEntryDetail
{
    public class CreateBookEntryDetailDto
    {
        public required int EntryID { get; set; }
        public required int BookID { get; set; }
        public int Quantity { get; set; }
    }
    



}