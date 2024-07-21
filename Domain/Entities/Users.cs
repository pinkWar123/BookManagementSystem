using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BookManagementSystem.Domain.Entities
{
    public enum Roles
    {
        Manager,
        Cashier,
        Customer,
        StoreKeeper
    }

    public class User : IdentityUser
    {
        [StringLength(100)]
        public required string FullName { get; set; }
    }
}
