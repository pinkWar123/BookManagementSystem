namespace BookManagementSystem.Application.Dtos.Invoice
{
    public class InvoiceDto
    {
        public int Id { get; set; }
        public required DateOnly InvoiceDate { get; set; }
        public int? CustomerID { get; set; }
        
    }
}