using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Book
{
    public class BookDto
    {
        public required string BookId { get; set; }
        public required string Title { get; set; }

        public required string Genre { get; set; }
        public required string Author { get; set; }

        public required int StockQuantity { get; set; }
        public required int Price { get; set; }
    }
}
