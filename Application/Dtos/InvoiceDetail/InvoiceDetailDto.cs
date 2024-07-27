namespace BookManagementSystem.Application.Dtos.InvoiceDetail
{
    //properties: InvoiceID, BookID, Quantity, Price
    public class InvoiceDetailDto
    {
        public int? InvoiceID { get; set; }
        public int? BookID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
    }
}