using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class BookEntry : Base
    {
        public required DateOnly Date { get; set; }
        public ICollection<BookEntryDetail>? BookEntryDetails { get; set; }
    }
}