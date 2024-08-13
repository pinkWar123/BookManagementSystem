namespace BookManagementSystem.Application.Dtos.InvoiceDetail
{
    //properties: InvoiceID, BookID, Quantity, Price
    public class CreateInvoiceDetailDto
    {
        public required int BookID { get; set; }
        public int Quantity { get; set; }
    }
}