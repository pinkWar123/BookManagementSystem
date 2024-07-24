namespace BookManagementSystem.Application.Dtos.InvoiceDetail
{
    public class CreateInvoiceDetailDto
    {
        public string? InvoiceID { get; set; }
        public string? InvoiceDate { get; set; }
        public string? CustomerID { get; set; }
    }
}