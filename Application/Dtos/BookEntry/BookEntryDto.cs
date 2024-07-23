using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.BookEntry
{
    //properties of the BookEntryDto: EntryID, BookID, quantity
    public class CreateBookEntryDto
    {
        public int quantity { get; set; }
    }

    public class UpdateBookEntryDto
    {
        public int quantity { get; set; }
    }
    public class BookEntryDto
    {
        public string? EntryID { get; set;}
        public string? BookID { get; set; }
        public int quantity { get; set; }
    }



}