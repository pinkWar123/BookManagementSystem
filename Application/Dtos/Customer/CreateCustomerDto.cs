namespace BookManagementSystem.Application.Dtos.Customer
{
    public class CreateCustomerDto
    {
        public string? CustomerName { get; set; }
        public int? TotalDebt { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Email { get; set; }
    }
}
