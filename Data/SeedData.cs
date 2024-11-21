using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using csiro_mvc.Models;

namespace csiro_mvc.Data
{
    public static class SeedData
    {
        public static async Task Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

            // Ensure database is created
            await context.Database.MigrateAsync();

            // Add roles if they don't exist
            string[] roles = { "Admin", "Applicant", "Researcher" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // Add admin user if it doesn't exist
            var adminEmail = "admin@csiro.au";
            if (await userManager.FindByEmailAsync(adminEmail) == null)
            {
                var admin = new ApplicationUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    FirstName = "Admin",
                    LastName = "User",
                    EmailConfirmed = true,
                    Department = "Administration",
                    Position = "System Administrator",
                    IsActive = true,
                    IsProfileComplete = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(admin, "Admin123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, "Admin");
                }
            }

            // Add test research programs if they don't exist
            if (!await context.ResearchPrograms.AnyAsync())
            {
                var programs = new[]
                {
                    new ResearchProgram { Title = "Climate Change Research", Description = "Research on climate change impacts", IsActive = true },
                    new ResearchProgram { Title = "Marine Biology Study", Description = "Study of marine ecosystems", IsActive = true },
                    new ResearchProgram { Title = "Renewable Energy Project", Description = "Research on sustainable energy", IsActive = true }
                };
                await context.ResearchPrograms.AddRangeAsync(programs);
                await context.SaveChangesAsync();
            }

            // Add test applicants and applications
            var testApplicantEmail = "applicant@test.com";
            if (await userManager.FindByEmailAsync(testApplicantEmail) == null)
            {
                var applicant = new ApplicationUser
                {
                    UserName = testApplicantEmail,
                    Email = testApplicantEmail,
                    FirstName = "Test",
                    LastName = "Applicant",
                    EmailConfirmed = true,
                    University = "Test University",
                    Qualification = "PhD in Environmental Science",
                    IsActive = true,
                    IsProfileComplete = true,
                    CreatedAt = DateTime.UtcNow
                };

                var result = await userManager.CreateAsync(applicant, "Test123!");
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(applicant, "Applicant");

                    // Create test applications
                    if (!await context.Applications.AnyAsync())
                    {
                        var programs = await context.ResearchPrograms.ToListAsync();
                        var applications = new[]
                        {
                            new Application
                            {
                                UserId = applicant.Id,
                                ResearchProgramId = programs[0].Id,
                                Status = ApplicationStatus.Submitted,
                                GPA = 3.8,
                                CreatedAt = DateTime.UtcNow.AddDays(-5)
                            },
                            new Application
                            {
                                UserId = applicant.Id,
                                ResearchProgramId = programs[1].Id,
                                Status = ApplicationStatus.UnderReview,
                                GPA = 3.5,
                                CreatedAt = DateTime.UtcNow.AddDays(-3)
                            }
                        };
                        await context.Applications.AddRangeAsync(applications);
                        await context.SaveChangesAsync();
                    }
                }
            }

            // Add default global settings if they don't exist
            if (!await context.GlobalSettings.AnyAsync())
            {
                var settings = new GlobalSetting
                {
                    Key = "MinimumGPA",
                    Value = "3.0"
                };
                await context.GlobalSettings.AddAsync(settings);
                await context.SaveChangesAsync();
            }
        }
    }
}
