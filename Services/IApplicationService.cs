using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace csiro_mvc.Services
{
    public interface IApplicationService
    {
        Task<Application?> GetApplicationByIdAsync(int id);
        Task<Application> CreateApplicationAsync(string userId, int programId, ApplicationForm form);
        Task<List<Application>> GetApplicationsAsync(string userId);
        Task<List<Application>> SearchApplicationsAsync(string searchTerm);
        Task<Application?> UpdateApplicationAsync(int id, Application application);
        Task<bool> DeleteApplicationAsync(int id);
        Task<int> GetTotalApplicationsCountAsync();
        Task<List<ApplicationStatusChangeViewModel>> GetRecentStatusChangesAsync(string userId);
        Task TrackStatusChangeAsync(int applicationId, ApplicationStatus? oldStatus, ApplicationStatus newStatus, string userId);
        Task<List<ApplicationStatusChangeViewModel>> GetStatusChangesAsync(string applicationId);
        
        // Admin methods
        Task<List<Application>> GetAllApplicationsAsync();
    }
}
