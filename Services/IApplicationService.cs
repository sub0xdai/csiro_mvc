using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId);
        Task<Application> CreateApplicationAsync(Application application);
        Task UpdateApplicationAsync(Application application);
        Task DeleteApplicationAsync(int id);
        Task<bool> ApplicationExistsAsync(int id);
    }
}
