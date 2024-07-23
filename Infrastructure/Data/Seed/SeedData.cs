using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace BookManagementSystem.Infrastructure.Data.Seed
{
    public class SeedData
    {
        public static async Task SeedEssentialsAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles if empty
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole(Roles.Manager.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Cashier.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.Customer.ToString()));
                await roleManager.CreateAsync(new IdentityRole(Roles.StoreKeeper.ToString()));
            }

            //Seed Default User if empty
            if (!userManager.Users.Any())
            {
                var defaultUsers = DefaultUsers.DefaultUserList;

                foreach (var defaultUser in defaultUsers)
                {
                    await userManager.CreateAsync(defaultUser, DefaultUsers.DefaultPassword);
                    await userManager.AddToRoleAsync(defaultUser, Roles.Manager.ToString());
                }
            }
        }
    }
}
