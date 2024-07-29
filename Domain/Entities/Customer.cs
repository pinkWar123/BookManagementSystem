namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Customer : Base
    {

        [StringLength(45)]
        public required string CustomerName { get; set; }

        public required int TotalDebt { get; set; }

        [StringLength(150)]
        public required string Address { get; set; }

        [StringLength(10)]
        [Column(TypeName = "varchar(10)")]
        public required string PhoneNumber { get; set; }

        [StringLength(150)]
        [Column(TypeName = "varchar(150)")]
        public string? Email { get; set; }

        public ICollection<PaymentReceipt>? PaymentReceipts { get; set; }

        public ICollection<DebtReportDetail>? DebtReportDetails { get; set; }

        public ICollection<Invoice>? Invoices { get; set; }


    }


}
