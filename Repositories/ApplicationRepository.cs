using csiro_mvc.Data;
using csiro_mvc.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace csiro_mvc.Repositories
{
    public class ApplicationRepository : GenericRepository<Application>, IApplicationRepository
    {
        private readonly IConfiguration _configuration;

        public ApplicationRepository(ApplicationDbContext context, IConfiguration configuration) 
            : base(context)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<Application>> GetTopApplicationsByGPAAsync(int count)
        {
            return await _dbSet
                .Include(a => a.User)
                .OrderByDescending(a => a.GPA)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId)
        {
            return await _dbSet
                .Include(a => a.User)
                .Where(a => a.UserId == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUniversityAsync(string university)
        {
            return await _dbSet
                .Include(a => a.User)
                .Where(a => a.University.ToLower().Contains(university.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByCourseTypeAsync(CourseType courseType)
        {
            return await _dbSet
                .Include(a => a.User)
                .Where(a => a.CourseType == courseType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByStatusAsync(ApplicationStatus status)
        {
            return await _dbSet
                .Include(a => a.User)
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public async Task<double> GetConfiguredGPACutoffAsync()
        {
            // First try to get from database configuration table
            // If not found, fall back to appsettings.json
            return double.Parse(_configuration["ApplicationSettings:GPACutoff"] ?? "3.0");
        }

        public async Task SetConfiguredGPACutoffAsync(double cutoff)
        {
            // In a real application, this would be stored in a database configuration table
            // For now, we'll just validate the input
            if (cutoff < 3.0 || cutoff > 4.0)
            {
                throw new ArgumentException("GPA cutoff must be between 3.0 and 4.0");
            }
        }
    }
}
