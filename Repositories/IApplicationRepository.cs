using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Repositories
{
    public interface IApplicationRepository
    {
        Task<Application?> GetByIdAsync(int id);
        Task<Application?> GetApplicationWithDetailsAsync(int id);
        Task<IEnumerable<Application>> GetAllAsync();
        Task<IEnumerable<Application>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Application>> SearchAsync(string searchTerm);
        Task<(IEnumerable<Application> Items, int TotalCount)> GetPaginatedApplicationsAsync(int pageNumber, int pageSize, string? searchTerm = null);
        Task<Application> AddAsync(Application application);
        Task<Application?> UpdateAsync(int id, Application application);
        Task<bool> DeleteAsync(int id);
    }
}
