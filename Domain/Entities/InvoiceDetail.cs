using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class InvoiceDetail
    {
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string InvoiceID { get; set; }
        
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string BookID { get; set; }
        public required int Quantity { get; set; }

        public required int Price { get; set; }

        [ForeignKey("InvoiceID")]
        public virtual Invoice Invoice { get; set; } 

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }


}
