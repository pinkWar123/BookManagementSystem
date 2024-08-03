using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Data.Repositories;

namespace BookManagementSystem.Application.Queries
{
    public class BookQuery : QueryObject
    {
        public  int? BookId { get; set; }
        public  string? Title { get; set; }

        public  string? Genre { get; set; }
        public  string? Author { get; set; }

    }
}