using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class BookEntryDetail
    {
        public int EntryID { get; set; }
        
        public int BookID { get; set; }
        [Required]
        public required int Quantity { get; set; }


        [ForeignKey("EntryID")]
        public virtual BookEntry BookEntry { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }


}
