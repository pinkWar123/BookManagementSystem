using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Book
{
    public class CreateBookDto
    {
        public required string Title { get; set; }
        public required string Genre { get; set; }
        public required string Author { get; set; }
        public required int Quantity { get; set; }
        public required int Price { get; set; }
    }
}
