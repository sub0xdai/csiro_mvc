using System.Threading.Tasks;
using System.Collections.Generic;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;

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
        Task<List<ResearchProgram>> GetAllProgramsAsync();
        Task<ResearchProgram> GetProgramByIdAsync(int id);
        Task<List<ResearchProgram>> GetRecentProgramsAsync(int count = 5);
        Task<int> GetUserApplicationsCountAsync(string userId);
        Task<int> GetTotalApplicationsCountAsync(string userId);
        Task<List<ApplicationStatusChange>> GetRecentStatusChangesAsync(string userId);
        Task TrackStatusChangeAsync(Application application, ApplicationStatus? oldStatus, string userId);
    }
}
