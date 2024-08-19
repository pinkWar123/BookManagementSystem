using System.ComponentModel.DataAnnotations;
using BookManagementSystem.Application.Dtos.BookEntryDetail;
namespace BookManagementSystem.Application.Dtos.BookEntry
{
    public class CreateBookEntryDto
    {
        [Required]
        public List<CreateBookEntryDetailDto> BookEntryDetails { get; set; }
        [Required]
        public string? EntryDate { get; set; }
    }

}