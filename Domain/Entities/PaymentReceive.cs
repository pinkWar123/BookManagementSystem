using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class PaymentReceive : Base
    {
        public required DateOnly Date { get; set; }
        public required int Amount { get; set; }
        
        [StringLength(5)]
        [Column(TypeName = "char(5)")]
        public required string CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; } 
    }
}