using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Services
{
    public interface IApplicationService
    {
        Task<IEnumerable<Application>> GetAllApplicationsAsync();
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId);
        Task<IEnumerable<Application>> SearchApplicationsAsync(string searchTerm);
        Task<Application> CreateApplicationAsync(Application application);
        Task<Application?> UpdateApplicationAsync(int id, Application application);
        Task<bool> DeleteApplicationAsync(int id);
    }
}
