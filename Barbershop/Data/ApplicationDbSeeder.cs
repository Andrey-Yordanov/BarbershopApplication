using Barbershop.Data.Models;
using Microsoft.AspNetCore.Identity;

namespace Barbershop.Data
{
    public class ApplicationDbSeeder
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            string[] roles = { "Admin", "User" };

            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                    await roleManager.CreateAsync(new IdentityRole(role));
            }

            string adminEmail = "admin@abv.bg";
            var admin = await userManager.FindByEmailAsync(adminEmail);
            if (admin == null)
            {
                admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User"
                };
                await userManager.CreateAsync(admin, "Admin123!");
                await userManager.AddToRoleAsync(admin, "Admin");
            }

            string testEmail = "user@abv.bg";
            var testUser = await userManager.FindByEmailAsync(testEmail);
            if (testUser == null)
            {
                testUser = new ApplicationUser
                {
                    UserName = testEmail,
                    Email = testEmail,
                    FirstName = "Test",
                    LastName = "User"
                };
                await userManager.CreateAsync(testUser, "User123!");
                await userManager.AddToRoleAsync(testUser, "User");
            }
        }
    }
}
