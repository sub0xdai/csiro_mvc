using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<IEnumerable<Application>> GetApplicationsAsync(string userId);
        Task<Application?> GetApplicationByIdAsync(int id);
        Task CreateApplicationAsync(Application application);
        Task UpdateApplicationAsync(Application application);
        Task DeleteApplicationAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId);
        Task<IEnumerable<Application>> SearchApplicationsAsync(string userId, string searchTerm);
    }
}
