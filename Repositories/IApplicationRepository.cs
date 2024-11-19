using csiro_mvc.Models;

namespace csiro_mvc.Repositories
{
    public interface IApplicationRepository : IGenericRepository<Application>
    {
        Task<IEnumerable<Application>> GetTopApplicationsByGPAAsync(int count);
        Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId);
        Task<IEnumerable<Application>> GetApplicationsByUniversityAsync(string university);
        Task<IEnumerable<Application>> GetApplicationsByCourseTypeAsync(CourseType courseType);
        Task<IEnumerable<Application>> GetApplicationsByStatusAsync(ApplicationStatus status);
        Task<double> GetConfiguredGPACutoffAsync();
        Task SetConfiguredGPACutoffAsync(double cutoff);
    }
}
