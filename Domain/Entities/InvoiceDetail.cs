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
        public required string InvoiceID { get; set; }
        [StringLength(5)]
        public required string BookID { get; set; }
        public required int quantity { get; set; }

        public required int price { get; set; }

        [ForeignKey("InvoiceID")]
        public virtual Invoice Invoice { get; set; } 

        [ForeignKey("BookID")]
        public virtual Book Book { get; set; }

    }


}
