using csiro_mvc.Data;
using csiro_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace csiro_mvc.Repositories
{
    public class ApplicationRepository : IApplicationRepository
    {
        private readonly ApplicationDbContext _context;

        public ApplicationRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Application?> GetByIdAsync(int id)
        {
            return await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Settings)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Settings)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> SearchAsync(string searchTerm)
        {
            var query = _context.Applications
                .Include(a => a.User)
                .Include(a => a.Settings)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a => 
                    a.Title.ToLower().Contains(searchTerm) || 
                    a.Description.ToLower().Contains(searchTerm));
            }

            return await query
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Application> AddAsync(Application application)
        {
            application.CreatedAt = DateTime.UtcNow;
            application.Status = ApplicationStatus.Pending;

            // Create ApplicationSettings if not provided
            if (application.Settings == null)
            {
                application.Settings = new ApplicationSettings
                {
                    NotificationsEnabled = true,
                    Theme = "Light",
                    Language = "English"
                };
            }

            _context.Applications.Add(application);
            await _context.SaveChangesAsync();
            return application;
        }

        public async Task<Application?> UpdateAsync(int id, Application application)
        {
            var existingApplication = await _context.Applications
                .Include(a => a.Settings)
                .FirstOrDefaultAsync(a => a.Id == id);

            if (existingApplication == null)
                return null;

            existingApplication.Title = application.Title;
            existingApplication.Description = application.Description;
            existingApplication.Status = application.Status;
            existingApplication.UpdatedAt = DateTime.UtcNow;

            if (application.Settings != null)
            {
                existingApplication.Settings.NotificationsEnabled = application.Settings.NotificationsEnabled;
                existingApplication.Settings.Theme = application.Settings.Theme;
                existingApplication.Settings.Language = application.Settings.Language;
            }

            await _context.SaveChangesAsync();
            return existingApplication;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var application = await _context.Applications.FindAsync(id);
            if (application == null)
                return false;

            _context.Applications.Remove(application);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<Application>> GetByUserIdAsync(string userId)
        {
            return await _context.Applications
                .Include(a => a.Settings)
                .Where(a => a.UserId == userId)
                .OrderByDescending(a => a.CreatedAt)
                .ToListAsync();
        }

        public async Task<Application?> GetApplicationWithDetailsAsync(int id)
        {
            return await _context.Applications
                .Include(a => a.User)
                .Include(a => a.Settings)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<(IEnumerable<Application> Items, int TotalCount)> GetPaginatedApplicationsAsync(
            int pageNumber, 
            int pageSize, 
            string? searchTerm = null)
        {
            var query = _context.Applications
                .Include(a => a.User)
                .Include(a => a.Settings)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                searchTerm = searchTerm.ToLower();
                query = query.Where(a => 
                    a.Title.ToLower().Contains(searchTerm) || 
                    a.Description.ToLower().Contains(searchTerm) ||
                    a.CourseType.ToLower().Contains(searchTerm));
            }

            var totalCount = await query.CountAsync();
            var items = await query
                .OrderByDescending(a => a.CreatedAt)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }
    }
}
