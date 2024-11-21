using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using csiro_mvc.Data;
using csiro_mvc.Models;
using csiro_mvc.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace csiro_mvc.Services
{
    public class ApplicationService : IApplicationService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<ApplicationService> _logger;

        public ApplicationService(
            ApplicationDbContext context,
            ILogger<ApplicationService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<List<Application>> GetApplicationsAsync(string userId)
        {
            try
            {
                return await _context.Applications
                    .Include(a => a.StatusHistory)
                    .Where(a => a.UserId == userId)
                    .OrderByDescending(a => a.CreatedAt)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving applications for user {UserId}", userId);
                throw;
            }
        }

        public async Task<Application?> GetApplicationByIdAsync(int id)
        {
            return await _context.Applications
                .Include(a => a.ResearchProgram)
                .Include(a => a.StatusHistory)
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<Application> CreateApplicationAsync(string userId, int programId, ApplicationForm form)
        {
            try
            {
                var researchProgram = await _context.ResearchPrograms.FindAsync(programId);
                if (researchProgram == null)
                {
                    throw new InvalidOperationException($"Research program with ID {programId} not found.");
                }

                string? cvFilePath = null;
                if (form.CVFile != null && form.CVFile.Length > 0)
                {
                    var uploadsDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
                    Directory.CreateDirectory(uploadsDir);

                    var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(form.CVFile.FileName)}";
                    cvFilePath = Path.Combine("uploads", uniqueFileName);
                    var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", cvFilePath);

                    using (var stream = new FileStream(fullPath, FileMode.Create))
                    {
                        await form.CVFile.CopyToAsync(stream);
                    }
                }

                var application = new Application
                {
                    UserId = userId,
                    ResearchProgramId = programId,
                    Title = researchProgram.Title,
                    ProgramTitle = researchProgram.Title,
                    CourseType = form.CourseType,
                    University = form.University,
                    GPA = form.GPA,
                    CoverLetter = form.CoverLetter,
                    CVFilePath = cvFilePath,
                    Status = ApplicationStatus.Pending,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                var statusHistory = new ApplicationStatusHistory
                {
                    ApplicationId = application.Id,
                    Status = ApplicationStatus.Pending,
                    OldStatus = ApplicationStatus.Draft,
                    ChangedAt = DateTime.UtcNow,
                    ChangedBy = userId,
                    Comment = "Application submitted"
                };

                application.StatusHistory = new List<ApplicationStatusHistory> { statusHistory };

                _context.Applications.Add(application);
                await _context.SaveChangesAsync();

                return application;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating application");
                throw;
            }
        }

        public async Task<Application?> UpdateApplicationAsync(int id, Application application)
        {
            try
            {
                var existingApplication = await _context.Applications.FindAsync(id);
                if (existingApplication == null)
                {
                    throw new KeyNotFoundException($"Application with ID {id} not found");
                }

                existingApplication.UpdatedAt = DateTime.UtcNow;
                existingApplication.Status = application.Status;
                existingApplication.ProgramTitle = application.ProgramTitle;
                existingApplication.CourseType = application.CourseType;

                await _context.SaveChangesAsync();
                return existingApplication;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application: {Id}", id);
                throw;
            }
        }

        public async Task<bool> DeleteApplicationAsync(int id)
        {
            try
            {
                var application = await _context.Applications.FindAsync(id);
                if (application == null)
                {
                    return false;
                }

                _context.Applications.Remove(application);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting application: {Id}", id);
                throw;
            }
        }

        public async Task<List<Application>> SearchApplicationsAsync(string searchTerm)
        {
            try
            {
                _logger.LogInformation("Searching applications with term: {SearchTerm}", searchTerm);
                return await _context.Applications
                    .Include(a => a.StatusHistory)
                    .Where(a => a.ProgramTitle.Contains(searchTerm) ||
                               a.CourseType.Contains(searchTerm))
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching applications with term: {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<int> GetTotalApplicationsCountAsync()
        {
            return await _context.Applications.CountAsync();
        }

        public async Task<List<ApplicationStatusChangeViewModel>> GetRecentStatusChangesAsync(string userId)
        {
            var recentChanges = await _context.Set<ApplicationStatusHistory>()
                .Include(sh => sh.Application)
                .Where(sh => sh.Application != null && sh.Application.UserId == userId)
                .OrderByDescending(sh => sh.ChangedAt)
                .Take(10)
                .Select(sh => new ApplicationStatusChangeViewModel
                {
                    ApplicationId = sh.ApplicationId,
                    Title = sh.Application != null ? sh.Application.ProgramTitle : string.Empty,
                    OldStatus = sh.OldStatus,
                    NewStatus = sh.Status,
                    ChangedAt = sh.ChangedAt
                })
                .ToListAsync();

            return recentChanges;
        }

        public async Task TrackStatusChangeAsync(int applicationId, ApplicationStatus? oldStatus, ApplicationStatus newStatus, string userId)
        {
            var statusHistory = new ApplicationStatusHistory
            {
                ApplicationId = applicationId,
                OldStatus = oldStatus ?? ApplicationStatus.Draft,
                Status = newStatus,
                ChangedBy = userId,
                ChangedAt = DateTime.UtcNow
            };

            _context.Set<ApplicationStatusHistory>().Add(statusHistory);
            await _context.SaveChangesAsync();
        }

        public async Task<List<ApplicationStatusChangeViewModel>> GetStatusChangesAsync(string userId)
        {
            try
            {
                var applications = await _context.Applications
                    .Include(a => a.StatusHistory)
                    .Where(a => a.UserId == userId)
                    .ToListAsync();

                var allStatusChanges = applications
                    .SelectMany(a => a.StatusHistory ?? new List<ApplicationStatusHistory>())
                    .OrderByDescending(h => h.ChangedAt)
                    .Select(h => new ApplicationStatusChangeViewModel
                    {
                        ApplicationId = h.ApplicationId,
                        ApplicationTitle = applications.FirstOrDefault(a => a.Id == h.ApplicationId)?.Title ?? string.Empty,
                        Title = applications.FirstOrDefault(a => a.Id == h.ApplicationId)?.Title ?? string.Empty,
                        OldStatus = h.OldStatus,
                        NewStatus = h.Status,
                        ChangedAt = h.ChangedAt
                    })
                    .ToList();

                return allStatusChanges;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving status changes for user {UserId}", userId);
                throw;
            }
        }

        public async Task<bool> UpdateStatusAsync(int applicationId, ApplicationStatus newStatus, string userId, string? comment = null)
        {
            var application = await _context.Applications
                .Include(a => a.StatusHistory)
                .FirstOrDefaultAsync(a => a.Id == applicationId);

            if (application == null)
                return false;

            var oldStatus = application.Status;
            application.Status = newStatus;

            var statusHistory = new ApplicationStatusHistory
            {
                ApplicationId = applicationId,
                OldStatus = oldStatus,
                Status = newStatus,
                ChangedAt = DateTime.UtcNow,
                ChangedBy = userId,
                Comment = comment
            };

            application.StatusHistory ??= new List<ApplicationStatusHistory>();
            application.StatusHistory.Add(statusHistory);

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating application status");
                return false;
            }
        }

        public async Task<List<Application>> GetAllApplicationsAsync()
        {
            return await _context.Applications
                .Include(a => a.StatusHistory)
                .ToListAsync();
        }
    }
}
