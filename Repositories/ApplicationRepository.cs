using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
                .Include(a => a.Settings)
                .OrderByDescending(a => a.GPA)
                .Take(count)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId)
        {
            return await _dbSet
                .Where(a => a.UserId == userId)
                .Include(a => a.User)
                .Include(a => a.Settings)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUniversityAsync(string university)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Settings)
                .Where(a => a.University.ToLower().Contains(university.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByCourseTypeAsync(CourseType courseType)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Settings)
                .Where(a => a.CourseType == courseType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Application>> GetApplicationsByStatusAsync(ApplicationStatus status)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Settings)
                .Where(a => a.Status == status)
                .ToListAsync();
        }

        public override async Task<Application?> GetByIdAsync(int id)
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Settings)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public override async Task<IEnumerable<Application>> GetAllAsync()
        {
            return await _dbSet
                .Include(a => a.User)
                .Include(a => a.Settings)
                .ToListAsync();
        }

        public Task<double> GetConfiguredGPACutoffAsync()
        {
            var cutoffStr = _configuration["ApplicationSettings:GPACutoff"];
            if (double.TryParse(cutoffStr, out double cutoff))
            {
                return Task.FromResult(cutoff);
            }
            return Task.FromResult(3.0); // Default value
        }

        public async Task SetConfiguredGPACutoffAsync(double cutoff)
        {
            if (cutoff < 0 || cutoff > 4.0)
            {
                throw new ArgumentException("GPA cutoff must be between 0 and 4.0");
            }
            // In a real application, this would update a settings table or configuration
            // For now, we'll just validate the input
            await Task.CompletedTask;
        }
    }
}
