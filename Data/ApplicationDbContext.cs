using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using csiro_mvc.Models;

namespace csiro_mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Application> Applications { get; set; }
        public DbSet<ApplicationSettings> ApplicationSettings { get; set; }
        public DbSet<ApplicationStatusHistory> ApplicationStatusHistories { get; set; }
        public DbSet<ResearchProgram> ResearchPrograms { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<GlobalSetting> GlobalSettings { get; set; }

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

            // Configure Application entity
            builder.Entity<Application>(entity =>
            {
                entity.Property(e => e.Status)
                    .HasDefaultValue(ApplicationStatus.Draft)
                    .IsRequired();

                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v)
                    .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                entity.HasOne(e => e.User)
                    .WithMany(e => e.Applications)
                    .HasForeignKey(e => e.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure ResearchProgram entity
            builder.Entity<ResearchProgram>(entity =>
            {
                entity.Property(e => e.CreatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");

                entity.Property(e => e.UpdatedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v,
                        v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);

                entity.Property(e => e.StartDate)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));

                entity.Property(e => e.EndDate)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
            });

            // Configure ApplicationStatusHistory entity
            builder.Entity<ApplicationStatusHistory>(entity =>
            {
                entity.Property(e => e.ChangedAt)
                    .HasColumnType("timestamp with time zone")
                    .HasConversion(
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc),
                        v => DateTime.SpecifyKind(v, DateTimeKind.Utc))
                    .HasDefaultValueSql("CURRENT_TIMESTAMP AT TIME ZONE 'UTC'");
            });

            // Configure indexes
            builder.Entity<Application>()
                .HasIndex(a => a.UserId);

            builder.Entity<Application>()
                .HasIndex(a => a.CreatedAt);

            builder.Entity<ApplicationUser>()
                .HasIndex(u => u.Department);

            // Seed research programs with proper UTC timestamps
            var now = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            builder.Entity<ResearchProgram>().HasData(
                new ResearchProgram
                {
                    Id = 1,
                    Title = "Advanced Machine Learning Research",
                    Description = "Research in advanced machine learning techniques focusing on deep learning and neural networks.",
                    OpenPositions = 2,
                    Department = "Computer Science",
                    Supervisor = "Dr. John Smith",
                    FundingAmount = 75000,
                    StartDate = now.AddMonths(1),
                    EndDate = now.AddYears(2),
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new ResearchProgram
                {
                    Id = 2,
                    Title = "Quantum Computing Applications",
                    Description = "Exploring practical applications of quantum computing in cryptography and optimization.",
                    OpenPositions = 3,
                    Department = "Physics",
                    Supervisor = "Dr. Sarah Johnson",
                    FundingAmount = 100000,
                    StartDate = now.AddMonths(2),
                    EndDate = now.AddYears(3),
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new ResearchProgram
                {
                    Id = 3,
                    Title = "Sustainable Energy Systems",
                    Description = "Research in renewable energy systems and smart grid technologies.",
                    OpenPositions = 4,
                    Department = "Engineering",
                    Supervisor = "Dr. Michael Brown",
                    FundingAmount = 120000,
                    StartDate = now.AddMonths(1),
                    EndDate = now.AddYears(2),
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new ResearchProgram
                {
                    Id = 4,
                    Title = "Data Science for Healthcare",
                    Description = "Applying data science techniques to improve healthcare outcomes and patient care.",
                    OpenPositions = 2,
                    Department = "Health Sciences",
                    Supervisor = "Dr. Emily Chen",
                    FundingAmount = 90000,
                    StartDate = now.AddMonths(3),
                    EndDate = now.AddYears(2),
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                },
                new ResearchProgram
                {
                    Id = 5,
                    Title = "Artificial Intelligence Ethics",
                    Description = "Research on ethical implications and governance of AI systems.",
                    OpenPositions = 3,
                    Department = "Philosophy",
                    Supervisor = "Dr. David Wilson",
                    FundingAmount = 80000,
                    StartDate = now.AddMonths(2),
                    EndDate = now.AddYears(2),
                    IsActive = true,
                    CreatedAt = now,
                    UpdatedAt = now
                }
            );
        }
    }
}