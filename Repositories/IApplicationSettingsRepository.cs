using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Repositories
{
    public interface IApplicationSettingsRepository
    {
        Task<ApplicationSettings?> GetByIdAsync(int id);
        Task<ApplicationSettings?> GetByApplicationIdAsync(int applicationId);
        Task<ApplicationSettings> AddAsync(ApplicationSettings settings);
        Task<ApplicationSettings?> UpdateAsync(int id, ApplicationSettings settings);
        Task<bool> DeleteAsync(int id);
    }
}
