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
        public required DateOnly date { get; set; }
        public required int amount { get; set; }
        [StringLength(5)]
        public required string customerID { get; set; }

        [ForeignKey("customerID")]
        public virtual Customer Customer { get; set; } 
    }
}