using Microsoft.AspNetCore.Identity;
using System;
using System.Threading.Tasks;
using WebAppIdentity.Models;

namespace WebAppIdentity.EF
{
    public static class AppDbInitializer
    {
        public static async Task InitializeAsync(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            string adminEmail = "admin@gmail.com";
            string password = "admin";

            if (await roleManager.FindByNameAsync("admin") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
            }
            if (await roleManager.FindByNameAsync("user") == null)
            {
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new User{
                    UserName = adminEmail,
                    FullName = "admin",
                    Email = "admin@gmail.com",
                    Gender = "male",
                    BirthDate = new DateTime(1990, 5, 15),
                    EmailConfirmed = true
                };

                IdentityResult result = await userManager.CreateAsync(admin, password);
                
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "admin");
                }
            }
        }
    }
}
