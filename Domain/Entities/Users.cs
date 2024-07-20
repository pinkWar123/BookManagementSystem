using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BookManagementSystem.Domain.Entities
{
    public enum Roles
    {
        Manager,
        Cashier,
        Customer,
        StoreKeeper
    }

    public class Users : IdentityUser
    {
        [StringLength(100)]
        public required string FullName { get; set; }

        public required Roles Role { get; set; }
    }
}
