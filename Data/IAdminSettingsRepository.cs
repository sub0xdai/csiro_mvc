using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Data
{
    public interface IAdminSettingsRepository
    {
        Task<List<GlobalSetting>> GetAllSettingsAsync();
        Task<GlobalSetting> UpdateSettingAsync(string key, string value);
        Task<GlobalSetting> GetSettingByKeyAsync(string key);
    }
}
