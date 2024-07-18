using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class Book
    {
        public int Id { get; set; } // ef core
        public required string Title { get; set; }
        public int Price { get; set; }
    }
}