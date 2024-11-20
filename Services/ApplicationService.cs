using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;
using csiro_mvc.Repositories;

namespace csiro_mvc.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ApplicationService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Application>> GetAllApplicationsAsync()
        {
            return await _unitOfWork.Applications.GetAllAsync();
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            return await _unitOfWork.Applications.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Application>> GetApplicationsByUserIdAsync(string userId)
        {
            return await _unitOfWork.Applications.GetApplicationsByUserIdAsync(userId);
        }

        public async Task<Application> CreateApplicationAsync(Application application)
        {
            await _unitOfWork.Applications.AddAsync(application);
            await _unitOfWork.SaveChangesAsync();
            return application;
        }

        public async Task UpdateApplicationAsync(Application application)
        {
            await _unitOfWork.Applications.UpdateAsync(application);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteApplicationAsync(int id)
        {
            await _unitOfWork.Applications.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> ApplicationExistsAsync(int id)
        {
            var application = await GetApplicationByIdAsync(id);
            return application != null;
        }
    }
}
