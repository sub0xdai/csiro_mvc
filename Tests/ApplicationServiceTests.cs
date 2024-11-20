using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Data;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using csiro_mvc.Services;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace csiro_mvc.Tests
{
    public class ApplicationServiceTests
    {
        private readonly ApplicationDbContext _context;
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly ApplicationService _service;

        public ApplicationServiceTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _context = new ApplicationDbContext(options);
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _service = new ApplicationService(_context, _mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetApplicationByIdAsync_ReturnsApplication_WhenApplicationExists()
        {
            // Arrange
            var application = new Application
            {
                Id = 1,
                UserId = "user1",
                Title = "Test Application",
                Description = "Test Description",
                CourseType = Course.DataScience,
                GPA = 3.5,
                University = "Test University",
                CoverLetter = "Test Cover Letter",
                Status = ApplicationStatus.Draft
            };

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();

            // Act
            var result = await _service.GetApplicationByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Test Application", result.Title);
        }

        [Fact]
        public async Task CreateApplicationAsync_CreatesNewApplication()
        {
            // Arrange
            var application = new Application
            {
                UserId = "user1",
                Title = "Test Application",
                Description = "Test Description",
                CourseType = Course.DataScience,
                GPA = 3.5,
                University = "Test University",
                CoverLetter = "Test Cover Letter",
                Status = ApplicationStatus.Draft
            };

            // Act
            await _service.CreateApplicationAsync(application);

            // Assert
            var result = await _context.Applications.FirstOrDefaultAsync(a => a.Title == "Test Application");
            Assert.NotNull(result);
            Assert.Equal("Test Description", result.Description);
        }

        [Fact]
        public async Task SearchApplicationsAsync_ReturnsMatchingApplications()
        {
            // Arrange
            var applications = new List<Application>
            {
                new Application
                {
                    UserId = "user1",
                    Title = "Data Science Application",
                    Description = "ML Research",
                    CourseType = Course.DataScience,
                    University = "MIT"
                },
                new Application
                {
                    UserId = "user1",
                    Title = "AI Application",
                    Description = "AI Research",
                    CourseType = Course.ArtificialIntelligence,
                    University = "Stanford"
                }
            };

            await _context.Applications.AddRangeAsync(applications);
            await _context.SaveChangesAsync();

            // Act
            var results = await _service.SearchApplicationsAsync("user1", "Data");

            // Assert
            Assert.Single(results);
            Assert.Contains(results, a => a.Title == "Data Science Application");
        }
    }
}
