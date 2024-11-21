using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using csiro_mvc.Models;
using Microsoft.EntityFrameworkCore;

namespace csiro_mvc.Data
{
    public class AdminSettingsRepository : IAdminSettingsRepository
    {
        private readonly ApplicationDbContext _context;

        public AdminSettingsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<GlobalSetting>> GetAllSettingsAsync()
        {
            return await _context.GlobalSettings.ToListAsync();
        }

        public async Task<GlobalSetting> UpdateSettingAsync(string key, string value)
        {
            var setting = await _context.GlobalSettings.FirstOrDefaultAsync(s => s.Key == key);
            if (setting == null)
            {
                setting = new GlobalSetting { Key = key, Value = value };
                _context.GlobalSettings.Add(setting);
            }
            else
            {
                setting.Value = value;
                _context.GlobalSettings.Update(setting);
            }
            await _context.SaveChangesAsync();
            return setting;
        }

        public async Task<GlobalSetting> GetSettingByKeyAsync(string key)
        {
            return await _context.GlobalSettings.FirstOrDefaultAsync(s => s.Key == key);
        }
    }
}
