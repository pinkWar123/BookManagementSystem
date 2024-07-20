using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    public class BookEntry
    {
        // create EntryID and EntryDate properties
        public string? EntryID { get; set; }
        public string? EntryDate { get; set; }
    }
}