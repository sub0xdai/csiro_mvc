using System;
using System.Threading.Tasks;
using csiro_mvc.Data;
using csiro_mvc.Models;
using csiro_mvc.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace csiro_mvc.Tests
{
    public class ApplicationServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationService> _logger;
        private readonly ApplicationService _service;

        public ApplicationServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);

            var loggerMock = new Mock<ILogger<ApplicationService>>();
            _logger = loggerMock.Object;

            _service = new ApplicationService(_context, _logger);
        }

        [Fact]
        public async Task CreateApplicationAsync_ValidForm_ReturnsApplication()
        {
            // Arrange
            var userId = "testUser";
            var programId = 1;
            var form = new ApplicationForm
            {
                ProgramTitle = "Test Program",
                CourseType = "PhD"
            };

            var program = new ResearchProgram
            {
                Id = programId,
                Title = "Test Program",
                Department = "Test Department",
                IsActive = true,
                OpenPositions = 1
            };

            await _context.ResearchPrograms.AddAsync(program);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.CreateApplicationAsync(userId, programId, form);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(form.ProgramTitle, result.ProgramTitle);
            Assert.Equal(form.CourseType, result.CourseType);
            Assert.Equal(ApplicationStatus.Draft, result.Status);
            Assert.Single(result.StatusHistory);
        }

        [Fact]
        public async Task GetApplicationsAsync_ReturnsUserApplications()
        {
            // Arrange
            var userId = "testUser";
            var application = new Application
            {
                UserId = userId,
                ProgramTitle = "Test Program",
                CourseType = "PhD",
                Status = ApplicationStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            // Act
            var results = await _service.GetApplicationsAsync(userId);

            // Assert
            Assert.Single(results);
            Assert.Equal(userId, results[0].UserId);
        }

        [Fact]
        public async Task UpdateApplicationAsync_ValidUpdate_ReturnsUpdatedApplication()
        {
            // Arrange
            var application = new Application
            {
                UserId = "testUser",
                ProgramTitle = "Test Program",
                CourseType = "PhD",
                Status = ApplicationStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            application.Status = ApplicationStatus.Submitted;

            // Act
            var result = await _service.UpdateApplicationAsync(application.Id, application);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(ApplicationStatus.Submitted, result.Status);
        }

        [Fact]
        public async Task DeleteApplicationAsync_ExistingApplication_ReturnsTrue()
        {
            // Arrange
            var application = new Application
            {
                UserId = "testUser",
                ProgramTitle = "Test Program",
                CourseType = "PhD",
                Status = ApplicationStatus.Draft,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            await _context.Applications.AddAsync(application);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.DeleteApplicationAsync(application.Id);

            // Assert
            Assert.True(result);
            Assert.Null(await _context.Applications.FindAsync(application.Id));
        }
    }
}
