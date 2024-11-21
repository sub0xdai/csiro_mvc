using System.Collections.Generic;
using System.Threading.Tasks;
using csiro_mvc.Models;

namespace csiro_mvc.Services
{
    public interface IAdminService
    {
        Task<List<Application>> GetAllApplicationsAsync();
        Task SendInterviewInvitationAsync(int applicationId);
        Task<double> GetMinimumGPARequirementAsync();
        Task UpdateMinimumGPARequirementAsync(double gpa);
    }
}
