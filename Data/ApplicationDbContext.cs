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
        }
    }
}