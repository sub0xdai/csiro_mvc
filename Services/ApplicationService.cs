using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using Serilog;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = Log.ForContext<ApplicationService>();
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            try
            {
                return await _unitOfWork.Applications.GetAllAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all applications");
                throw;
            }
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            try
            {
                return await _unitOfWork.Applications.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting application with id: {Id}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId)
        {
            try
            {
                return await _unitOfWork.Applications.GetByUserIdAsync(userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting applications for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<IEnumerable<Application>> SearchApplicationsAsync(string searchTerm)
        {
            try
            {
                return await _unitOfWork.Applications.SearchAsync(searchTerm);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error searching applications with search term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<Application> CreateApplicationAsync(Application application)
        {
            try
            {
                var createdApplication = await _unitOfWork.Applications.AddAsync(application);
                await _unitOfWork.SaveAsync();
                return createdApplication;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating application");
                throw;
            }
        }

        public async Task<Application?> UpdateApplicationAsync(int id, Application application)
        {
            try
            {
                return await _unitOfWork.Applications.UpdateAsync(id, application);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating application with id: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            try
            {
                return await _unitOfWork.Applications.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting application with id: {Id}", id);
                throw;
            }
        }
    }
}
