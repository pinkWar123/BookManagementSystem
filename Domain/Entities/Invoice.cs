

namespace BookManagementSystem.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class Invoice : Base
    {
        public required DateOnly Date { get; set; }

        public int CustomerID { get; set; }

        [ForeignKey("CustomerID")]
        public virtual Customer Customer { get; set; }

        public ICollection<InvoiceDetail>? InvoiceDetails { get; set; }
    }
}
