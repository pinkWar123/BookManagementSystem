using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;

    public class Customer : Base
    {
        [StringLength(100)]
        public required string title { get; set; }

        [StringLength(45)]
        public required string customerName { get; set; }

        public required int totalDept { get; set; }

        [StringLength(150)]
        public required string address { get; set; }
        [StringLength(10)]
        public required string phoneNumber { get; set; }

        [StringLength(150)]
        public string? email { get; set; }

        public ICollection<PaymentReceive>? paymentReceives { get; set; }

        public ICollection<DeptReportDetail>? DeptReportDetails { get; set; }

        public ICollection<Invoice>? invoices { get; set; }

        
    }


}