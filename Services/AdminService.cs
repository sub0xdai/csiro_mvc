using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using csiro_mvc.Models;
using csiro_mvc.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using ILogger = Serilog.ILogger;
using Serilog;

namespace csiro_mvc.Services
{
    public class AdminService : IAdminService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger _logger;
        private readonly INotificationService _notificationService;
        private readonly IAdminSettingsRepository _adminSettings;

        public AdminService(
            ApplicationDbContext context,
            INotificationService notificationService,
            IAdminSettingsRepository adminSettings)
        {
            _context = context;
            _notificationService = notificationService;
            _adminSettings = adminSettings;
            _logger = Log.ForContext<AdminService>();
        }

        public async Task<List<Application>> GetAllApplicationsAsync()
        {
            try
            {
                return await _context.Applications
                    .Include(a => a.ResearchProgram)
                    .Include(a => a.User)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving all applications");
                throw;
            }
        }

        public async Task<double> GetMinimumGPARequirementAsync()
        {
            try
            {
                var setting = await _adminSettings.GetSettingByKeyAsync("MinimumGPA");
                if (setting != null && double.TryParse(setting.Value, out double gpa))
                {
                    return gpa;
                }
                return 3.0; // Default GPA requirement
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error retrieving minimum GPA requirement");
                throw;
            }
        }

        public async Task UpdateMinimumGPARequirementAsync(double gpa)
        {
            try
            {
                await _adminSettings.UpdateSettingAsync("MinimumGPA", gpa.ToString());
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error updating minimum GPA requirement");
                throw;
            }
        }

        public async Task SendInterviewInvitationAsync(int applicationId)
        {
            try
            {
                var application = await _context.Applications
                    .Include(a => a.User)
                    .FirstOrDefaultAsync(a => a.Id == applicationId);

                if (application == null)
                {
                    throw new ArgumentException("Application not found", nameof(applicationId));
                }

                // Send interview invitation
                await _notificationService.SendInterviewInvitationAsync(application);

                // Update application status
                application.Status = ApplicationStatus.UnderReview;
                _context.Applications.Update(application);
                await _context.SaveChangesAsync();

                _logger.Information("Interview invitation sent and status updated for application {ApplicationId}", applicationId);
            }
            catch (Exception ex)
            {
                _logger.Error(ex, "Error sending interview invitation for application {ApplicationId}", applicationId);
                throw;
            }
        }
    }
}
