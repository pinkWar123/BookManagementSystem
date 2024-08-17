using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Book
{
    public class UpdateBookDto
    {
        public string? Title { get; set; }

        public  string? Genre { get; set; }
        public  string? Author { get; set; }
        public string? ImagePath { get; set; }

        public  int? StockQuantity { get; set; }
        public  int? Price { get; set; }
    }
}
