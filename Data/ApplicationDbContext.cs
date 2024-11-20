using csiro_mvc.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace csiro_mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; } = null!;
        public DbSet<ResearchProgram> ResearchPrograms { get; set; } = null!;
        public DbSet<ApplicationSettings> ApplicationSettings { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<ApplicationUser>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Department)
                    .HasMaxLength(100);

                entity.Property(e => e.Position)
                    .HasMaxLength(100);

                entity.Property(e => e.Qualification)
                    .HasMaxLength(200);

                entity.Property(e => e.University)
                    .HasMaxLength(200);
            });

            // Configure Application-User relationship
            builder.Entity<Application>()
                .HasOne(a => a.User)
                .WithMany(u => u.Applications)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Configure Application-Settings relationship
            builder.Entity<Application>()
                .HasOne(a => a.Settings)
                .WithOne(s => s.Application)
                .HasForeignKey<ApplicationSettings>(s => s.ApplicationId);

            // Configure indexes
            builder.Entity<Application>()
                .HasIndex(a => a.UserId);

            builder.Entity<Application>()
                .HasIndex(a => a.CreatedAt);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Department);

            // Configure default values
            builder.Entity<Application>()
                .Property(a => a.Status)
                .HasDefaultValue(ApplicationStatus.Pending);

            builder.Entity<Application>()
                .Property(a => a.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Seed Research Programs
            builder.Entity<ResearchProgram>().HasData(
                new ResearchProgram 
                { 
                    Id = 1, 
                    Name = "Data61", 
                    Description = "Digital and data innovation for Australia's digital future", 
                    OpenPositions = 5 
                },
                new ResearchProgram 
                { 
                    Id = 2, 
                    Name = "Space and Astronomy", 
                    Description = "Unlocking the secrets of the universe and supporting Australia's space industry", 
                    OpenPositions = 3 
                },
                new ResearchProgram 
                { 
                    Id = 3, 
                    Name = "Energy", 
                    Description = "Developing sustainable energy solutions for a cleaner future", 
                    OpenPositions = 4 
                },
                new ResearchProgram 
                { 
                    Id = 4, 
                    Name = "Manufacturing", 
                    Description = "Advanced manufacturing technologies and processes", 
                    OpenPositions = 2 
                },
                new ResearchProgram 
                { 
                    Id = 5, 
                    Name = "Health and Biosecurity", 
                    Description = "Improving health outcomes and protecting Australia's biosecurity", 
                    OpenPositions = 6 
                }
            );
        }
    }
}