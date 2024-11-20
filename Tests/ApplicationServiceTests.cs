using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using csiro_mvc.Services;
using Moq;
using Xunit;

namespace csiro_mvc.Tests
{
    public class ApplicationServiceTests
    {
        private readonly Mock<IUnitOfWork> _mockUnitOfWork;
        private readonly Mock<IApplicationRepository> _mockApplicationRepository;
        private readonly ApplicationService _applicationService;

        public ApplicationServiceTests()
        {
            _mockUnitOfWork = new Mock<IUnitOfWork>();
            _mockApplicationRepository = new Mock<IApplicationRepository>();
            _mockUnitOfWork.Setup(uow => uow.Applications).Returns(_mockApplicationRepository.Object);
            _applicationService = new ApplicationService(_mockUnitOfWork.Object);
        }

        [Fact]
        public async Task GetApplicationByIdAsync_ShouldReturnApplication()
        {
            // Arrange
            var application = new Application { Id = 1, Title = "Test App" };
            _mockApplicationRepository.Setup(repo => repo.GetByIdAsync(1))
                .ReturnsAsync(application);

            // Act
            var result = await _applicationService.GetApplicationByIdAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(application.Title, result.Title);
        }

        [Fact]
        public async Task CreateApplicationAsync_ShouldAddApplication()
        {
            // Arrange
            var application = new Application { Title = "New App" };
            _mockApplicationRepository.Setup(repo => repo.AddAsync(It.IsAny<Application>()))
                .ReturnsAsync(application);
            _mockUnitOfWork.Setup(uow => uow.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _applicationService.CreateApplicationAsync(application);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(application.Title, result.Title);
            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task UpdateApplicationAsync_ShouldUpdateApplication()
        {
            // Arrange
            var application = new Application { Id = 1, Title = "Updated App" };
            _mockApplicationRepository.Setup(repo => repo.UpdateAsync(1, application))
                .ReturnsAsync(application);
            _mockUnitOfWork.Setup(uow => uow.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _applicationService.UpdateApplicationAsync(1, application);

            // Assert
            Assert.NotNull(result);
            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteApplicationAsync_ShouldDeleteApplication()
        {
            // Arrange
            _mockApplicationRepository.Setup(repo => repo.DeleteAsync(1))
                .ReturnsAsync(true);
            _mockUnitOfWork.Setup(uow => uow.SaveAsync())
                .ReturnsAsync(1);

            // Act
            var result = await _applicationService.DeleteApplicationAsync(1);

            // Assert
            Assert.True(result);
            _mockUnitOfWork.Verify(uow => uow.SaveAsync(), Times.Once);
        }

        [Fact]
        public async Task GetAllApplicationsAsync_ShouldReturnAllApplications()
        {
            // Arrange
            var applications = new List<Application>
            {
                new Application { Id = 1, Title = "App 1" },
                new Application { Id = 2, Title = "App 2" }
            };

            _mockApplicationRepository.Setup(repo => repo.GetAllAsync())
                .ReturnsAsync(applications);

            // Act
            var result = await _applicationService.GetAllApplicationsAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(2, applications.Count);
        }
    }
}
