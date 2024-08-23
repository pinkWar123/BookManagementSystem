using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookManagementSystem.Application.Dtos.Regulation;
using BookManagementSystem.Application.Interfaces;
using BookManagementSystem.Application.Queries;
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

        public static async Task SeedRegulations(IRegulationService regulationService)
        {
            var regulationQuery = new RegulationQuery
            {
                PageNumber = 1,
                PageSize = 5
            };
            var regulation = await regulationService.GetAllRegulations(regulationQuery);
            if(regulation == null || regulation.Count() == 0)
            {
                foreach(var newRegulation in DefaultRegulations.DefaultRegulationList)
                {
                    var createRegulationDto = new CreateRegulationDto
                    {
                        Code = newRegulation.Code,
                        Value = newRegulation.Value,
                        Status = newRegulation.Status,
                        Content = newRegulation.Content
                    };
                    await regulationService.CreateRegulation(createRegulationDto);
                }
            }
        }
    }
}
