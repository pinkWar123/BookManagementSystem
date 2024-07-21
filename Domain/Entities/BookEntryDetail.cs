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
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string EntryID { get; set; }
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string BookID { get; set; }
        [Required]
        public required int Quantity { get; set; }


        [ForeignKey("EntryID")]
        public virtual BookEntry BookEntry { get; set; }

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }


}
