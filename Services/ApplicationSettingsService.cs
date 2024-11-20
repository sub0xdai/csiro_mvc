using System.Threading.Tasks;
using System.Linq;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using Serilog;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;

namespace csiro_mvc.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private IApplicationSettingsRepository ApplicationSettings => _unitOfWork.ApplicationSettings;

        public ApplicationSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _logger = Log.ForContext<ApplicationSettingsService>();
        }

        public async Task<ApplicationSettings?> GetSettingsByApplicationIdAsync(int applicationId)
        {
            try
            {
                return await ApplicationSettings.GetByApplicationIdAsync(applicationId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error getting settings for application: {ApplicationId}", applicationId);
                throw;
            }
        }

        public async Task<ApplicationSettings> CreateSettingsAsync(ApplicationSettings settings)
        {
            try
            {
                var result = await ApplicationSettings.AddAsync(settings);
                await _unitOfWork.SaveAsync();
                return result;
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error creating settings for application: {ApplicationId}", settings.ApplicationId);
                throw;
            }
        }

        public async Task UpdateSettingsAsync(ApplicationSettings settings)
        {
            try
            {
                var result = await ApplicationSettings.UpdateAsync(settings.Id, settings);
                if (result != null)
                {
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating settings for application: {ApplicationId}", settings.ApplicationId);
                throw;
            }
        }

        public async Task DeleteSettingsAsync(int applicationId)
        {
            try
            {
                var result = await ApplicationSettings.DeleteAsync(applicationId);
                if (result)
                {
                    await _unitOfWork.SaveAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error deleting settings for application: {ApplicationId}", applicationId);
                throw;
            }
        }

        public async Task<bool> SettingsExistAsync(int applicationId)
        {
            var settings = await GetSettingsByApplicationIdAsync(applicationId);
            return settings != null;
        }
    }
}
