namespace BookManagementSystem.Application.Dtos.Invoice
{
    public class CreateInvoiceDto
    {
        public int? InvoiceID { get; set; }
        public string? InvoiceDate { get; set; }
        public int? CustomerID { get; set; }
    }
    
}