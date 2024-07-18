using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Application.Dtos.Book
{
    public class CreateBookDto
    {
        public string? Title { get; set; }
        public int Price { get; set; }
    }
}