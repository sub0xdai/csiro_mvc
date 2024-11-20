using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using csiro_mvc.Data;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;

        public ApplicationService(ApplicationDbContext context, IUnitOfWork unitOfWork)
        {
            _context = context;
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

        public async Task<IEnumerable<Application>> GetApplicationsAsync(string userId)
        {
            return await _context.Applications
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
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

        public async Task<Application?> GetApplicationByIdAsyncDbContext(int id)
        {
            return await _context.Applications.FindAsync(id);
        }

        public async Task CreateApplicationAsync(Application application)
        {
            try
            {
                if (application == null)
                    throw new ArgumentNullException(nameof(application));

                application.CreatedAt = DateTime.UtcNow;
                application.UpdatedAt = DateTime.UtcNow;
                
                _context.Applications.Add(application);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating application");
                throw;
            }
        }

        public async Task<Application> CreateApplicationAsyncUnitOfWork(Application application)
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

        public async Task UpdateApplicationAsync(Application application)
        {
            try
            {
                if (application == null)
                    throw new ArgumentNullException(nameof(application));

                application.UpdatedAt = DateTime.UtcNow;
                
                _context.Entry(application).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating application");
                throw;
            }
        }

        public async Task<Application?> UpdateApplicationAsyncUnitOfWork(int id, Application application)
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

        public async Task DeleteApplicationAsync(int id)
        {
            try
            {
                var application = await _context.Applications.FindAsync(id);
                if (application != null)
                {
                    _context.Applications.Remove(application);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting application with id: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteApplicationAsyncUnitOfWork(int id)
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

        public async Task<IEnumerable<Application>> SearchApplicationsAsync(string userId, string searchTerm)
        {
            var query = _context.Applications.Where(a => a.UserId == userId);

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a => 
                    a.University.ToLower().Contains(searchTerm) ||
                    a.CourseType.ToString().ToLower().Contains(searchTerm) ||
                    a.CoverLetter.ToLower().Contains(searchTerm)
                );
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }
    }
}
