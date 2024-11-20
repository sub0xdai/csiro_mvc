using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Services
{
    public interface IApplicationSettingsService
    {
        Task<ApplicationSettings?> GetSettingsByApplicationIdAsync(int applicationId);
        Task<ApplicationSettings> CreateSettingsAsync(ApplicationSettings settings);
        Task UpdateSettingsAsync(ApplicationSettings settings);
        Task DeleteSettingsAsync(int applicationId);
        Task<bool> SettingsExistAsync(int applicationId);
    }
}
