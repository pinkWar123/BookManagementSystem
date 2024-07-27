namespace BookManagementSystem.Application.Dtos.InvoiceDetail
{
    //properties: InvoiceID, BookID, Quantity, Price
    public class CreateInvoiceDetailDto
    {
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}