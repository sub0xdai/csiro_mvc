using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using csiro_mvc.Data;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;
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
                _logger.Information("Getting all applications");
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
            try
            {
                _logger.Information("Getting applications for user: {UserId}", userId);
                var applications = await _context.Applications
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();
                
                _logger.Information("Found {Count} applications for user", applications.Count);
                return applications;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting applications for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            try
            {
                _logger.Information("Getting application by ID: {Id}", id);
                return await _context.Applications
                    .Include(a => a.StatusHistory)
                    .FirstOrDefaultAsync(a => a.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting application by ID: {Id}", id);
                throw;
            }
        }

        public async Task<Application?> GetApplicationByIdAsyncDbContext(int id)
        {
            try
            {
                _logger.Information("Getting application by ID: {Id} using DbContext", id);
                return await _context.Applications.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting application by ID: {Id} using DbContext", id);
                throw;
            }
        }

        public async Task CreateApplicationAsync(Application application)
        {
            try
            {
                _logger.Information("Starting CreateApplicationAsync for user: {UserId}", application.UserId);

                if (application == null)
                {
                    _logger.Error("Application object is null");
                    throw new ArgumentNullException(nameof(application));
                }

                application.CreatedAt = DateTime.UtcNow;
                application.UpdatedAt = DateTime.UtcNow;
                
                _logger.Information("Adding application to context");
                _context.Applications.Add(application);
                
                _logger.Information("Saving changes to database");
                await _context.SaveChangesAsync();
                
                _logger.Information("Application saved successfully with ID: {ApplicationId}", application.Id);
                
                // Track initial status
                await TrackStatusChangeAsync(application, null, application.UserId);
                
                _logger.Information("Status change tracked successfully");
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Database error occurred while creating application");
                throw;
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
                _logger.Information("Starting CreateApplicationAsyncUnitOfWork for user: {UserId}", application.UserId);
                var createdApplication = await _unitOfWork.Applications.AddAsync(application);
                await _unitOfWork.SaveAsync();
                _logger.Information("Application saved successfully with ID: {ApplicationId}", createdApplication.Id);
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
                _logger.Information("Starting UpdateApplicationAsync for application ID: {Id}", application.Id);

                if (application == null)
                {
                    _logger.Error("Application object is null");
                    throw new ArgumentNullException(nameof(application));
                }

                application.UpdatedAt = DateTime.UtcNow;
                
                _logger.Information("Updating application in context");
                _context.Entry(application).State = EntityState.Modified;
                
                _logger.Information("Saving changes to database");
                await _context.SaveChangesAsync();
                
                _logger.Information("Application updated successfully with ID: {ApplicationId}", application.Id);
            }
            catch (DbUpdateException ex)
            {
                _logger.Error(ex, "Database error occurred while updating application");
                throw;
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
                _logger.Information("Starting UpdateApplicationAsyncUnitOfWork for application ID: {Id}", id);
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
                _logger.Information("Deleting application with ID: {Id}", id);
                var application = await _context.Applications.FindAsync(id);
                if (application != null)
                {
                    _logger.Information("Removing application from context");
                    _context.Applications.Remove(application);
                    
                    _logger.Information("Saving changes to database");
                    await _context.SaveChangesAsync();
                    
                    _logger.Information("Application deleted successfully with ID: {ApplicationId}", id);
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
                _logger.Information("Deleting application with ID: {Id} using UnitOfWork", id);
                return await _unitOfWork.Applications.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting application with id: {Id} using UnitOfWork", id);
                throw;
            }
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId)
        {
            try
            {
                _logger.Information("Getting applications for user: {UserId}", userId);
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
                _logger.Information("Searching applications with search term: {SearchTerm}", searchTerm);
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
            try
            {
                _logger.Information("Searching applications for user: {UserId} with search term: {SearchTerm}", userId, searchTerm);
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
            catch (Exception ex)
            {
                _logger.Error(ex, "Error searching applications for user: {UserId} with search term: {SearchTerm}", userId, searchTerm);
                throw;
            }
        }

        public async Task<List<ResearchProgram>> GetRecentProgramsAsync(int count = 5)
        {
            try
            {
                _logger.Information("Getting recent research programs");
                return await _context.ResearchPrograms
                    .OrderByDescending(p => p.Id)
                    .Take(count)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting recent research programs");
                throw;
            }
        }

        public async Task<List<ResearchProgram>> GetAllProgramsAsync()
        {
            try
            {
                _logger.Information("Getting all research programs");
                return await _context.ResearchPrograms
                    .OrderBy(p => p.Name)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting all research programs");
                throw;
            }
        }

        public async Task<ResearchProgram> GetProgramByIdAsync(int id)
        {
            try
            {
                _logger.Information("Getting research program by ID: {Id}", id);
                return await _context.ResearchPrograms
                    .FirstOrDefaultAsync(p => p.Id == id);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting research program by ID: {Id}", id);
                throw;
            }
        }

        public async Task<int> GetUserApplicationsCountAsync(string userId)
        {
            try
            {
                _logger.Information("Getting user applications count for user: {UserId}", userId);
                return await _context.Applications
                    .CountAsync(a => a.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting user applications count for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<int> GetTotalApplicationsCountAsync(string userId)
        {
            try
            {
                _logger.Information("Getting total applications count for user: {UserId}", userId);
                return await _context.Applications
                    .CountAsync(a => a.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting total applications count for user: {UserId}", userId);
                throw;
            }
        }

        public async Task<List<ApplicationStatusChange>> GetRecentStatusChangesAsync(string userId)
        {
            try
            {
                _logger.Information("Getting recent status changes for user: {UserId}", userId);
                var recentChanges = await _context.ApplicationStatusHistory
                    .Include(h => h.Application)
                    .Where(h => h.Application.UserId == userId)
                    .OrderByDescending(h => h.ChangedAt)
                    .Take(5)
                    .Select(h => new ApplicationStatusChange
                    {
                        ApplicationId = h.ApplicationId,
                        ApplicationTitle = h.Application.Title,
                        OldStatus = h.OldStatus,
                        NewStatus = h.NewStatus,
                        ChangedAt = h.ChangedAt
                    })
                    .ToListAsync();

                return recentChanges;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting recent status changes for user: {UserId}", userId);
                throw;
            }
        }

        public async Task TrackStatusChangeAsync(Application application, ApplicationStatus? oldStatus, string userId)
        {
            try
            {
                _logger.Information("Tracking status change for application ID: {Id}", application.Id);
                var statusHistory = new ApplicationStatusHistory
                {
                    ApplicationId = application.Id,
                    OldStatus = oldStatus ?? ApplicationStatus.Draft,
                    NewStatus = application.Status,
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = userId
                };

                _logger.Information("Adding status history to context");
                _context.ApplicationStatusHistory.Add(statusHistory);
                
                _logger.Information("Saving changes to database");
                await _context.SaveChangesAsync();
                
                _logger.Information("Status change tracked successfully");
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error tracking status change for application ID: {Id}", application.Id);
                throw;
            }
        }
    }
}
