using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using csiro_mvc.Models;
using Microsoft.Extensions.Logging;

namespace csiro_mvc.Data
{
    public static class DbInitializer
    {
        public static async Task Initialize(IServiceProvider serviceProvider, ILogger logger)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

                try
                {
                    // Ensure database is created
                    context.Database.EnsureCreated();

                    // Seed Roles
                    string[] roleNames = { "Admin", "Researcher", "Applicant" };
                    foreach (var roleName in roleNames)
                    {
                        if (!await roleManager.RoleExistsAsync(roleName))
                        {
                            await roleManager.CreateAsync(new IdentityRole(roleName));
                            logger.LogInformation("Created role: {Role}", roleName);
                        }
                    }

                    // Check if we need to create an admin user
                    if (!await userManager.Users.AnyAsync())
                    {
                        var adminUser = new ApplicationUser
                        {
                            UserName = "admin@csiro.au",
                            Email = "admin@csiro.au",
                            FirstName = "Admin",
                            LastName = "User",
                            EmailConfirmed = true,
                            Department = "Administration",
                            Position = "System Administrator",
                            Qualification = "N/A",
                            University = "N/A"
                        };

                        var result = await userManager.CreateAsync(adminUser, "Admin123!");
                        if (result.Succeeded)
                        {
                            await userManager.AddToRoleAsync(adminUser, "Admin");
                            logger.LogInformation("Created admin user: {Email}", adminUser.Email);
                        }
                        else
                        {
                            foreach (var error in result.Errors)
                            {
                                logger.LogError("Error creating admin user: {Error}", error.Description);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, "An error occurred while seeding the database.");
                    throw;
                }
            }
        }
    }
}
