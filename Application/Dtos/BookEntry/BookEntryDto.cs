using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Application.Dtos.BookEntry
{
    public class BookEntryDto
    {
        public required int Id { get; set; }
        [Required]
        public DateOnly? EntryDate { get; set; }
    }
}