namespace BookManagementSystem.Application.Dtos.Invoice
{
    public class InvoiceDto
    {
        public int InvoiceID { get; set; }
        public DateOnly? InvoiceDate { get; set; }
        public int? CustomerID { get; set; }
        
    }
}