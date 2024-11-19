// Data/ApplicationDbContext.cs
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using csiro_mvc.Models;

namespace csiro_mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Configure User entity
            builder.Entity<User>(entity =>
            {
                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.GPA)
                    .IsRequired();

                entity.Property(e => e.University)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Qualification)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedAt)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            });

            // Configure Application entity
            builder.Entity<Application>(entity =>
            {
                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.Cascade);

                entity.Property(e => e.ApplicationDate)
                    .IsRequired()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValue(ApplicationStatus.Pending);

                entity.Property(e => e.University)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.CoverLetter)
                    .IsRequired();

                entity.Property(e => e.CVFilePath)
                    .HasMaxLength(500);
            });
        }
    }
}