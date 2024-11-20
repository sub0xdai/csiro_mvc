using System.Threading.Tasks;
using System.Linq;
using csiro_mvc.Models;
using csiro_mvc.Repositories;
using Microsoft.EntityFrameworkCore;

namespace csiro_mvc.Services
{
    public class ApplicationSettingsService : IApplicationSettingsService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationSettingsService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApplicationSettings?> GetSettingsByApplicationIdAsync(int applicationId)
        {
            var repository = _unitOfWork.GetRepository<ApplicationSettings>();
            var settings = await repository.FindAsync(s => s.ApplicationId == applicationId);
            return settings.FirstOrDefault();
        }

        public async Task<ApplicationSettings> CreateSettingsAsync(ApplicationSettings settings)
        {
            var repository = _unitOfWork.GetRepository<ApplicationSettings>();
            await repository.AddAsync(settings);
            await _unitOfWork.SaveChangesAsync();
            return settings;
        }

        public async Task UpdateSettingsAsync(ApplicationSettings settings)
        {
            var repository = _unitOfWork.GetRepository<ApplicationSettings>();
            await repository.UpdateAsync(settings);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteSettingsAsync(int applicationId)
        {
            var repository = _unitOfWork.GetRepository<ApplicationSettings>();
            await repository.DeleteAsync(applicationId);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> SettingsExistAsync(int applicationId)
        {
            var settings = await GetSettingsByApplicationIdAsync(applicationId);
            return settings != null;
        }
    }
}
