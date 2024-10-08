namespace BookManagementSystem.Application.Dtos.Customer
{
    public class CustomerDtoWithAmount
    {
        public int Id { get; set; }
        public required string CustomerName { get; set; }
        public int TotalDebt { get; set; }
        public required string Address { get; set; }
        public required string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public int TotalAmount { get; set; }
    }
}
