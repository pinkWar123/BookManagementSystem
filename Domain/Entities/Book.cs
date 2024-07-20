using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class Book
    {
        // Properties: BookID, Title, Genre , Author, StockQUantity, Price
        public string? BookID { get; set; }
        public string? Title { get; set; }
        public string? Genre { get; set; }
        public string? Author { get; set; }
        public int StockQuantity { get; set; }
        public double Price { get; set; }
        
    }
}