using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using csiro_mvc.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace csiro_mvc.Services
{
    public interface INotificationService
    {
        Task SendInterviewInvitationAsync(Application application);
    }

    public class NotificationService : INotificationService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<NotificationService> _logger;
        private readonly IWebHostEnvironment _environment;

        public NotificationService(
            IConfiguration configuration,
            ILogger<NotificationService> logger,
            IWebHostEnvironment environment)
        {
            _configuration = configuration;
            _logger = logger;
            _environment = environment;
        }

        public async Task SendInterviewInvitationAsync(Application application)
        {
            if (application.User == null)
            {
                throw new ArgumentException("Application must include User details");
            }

            var emailSubject = "Interview Invitation - CSIRO Research Program";
            var emailBody = $@"
                <html>
                <body>
                    <h2>Interview Invitation</h2>
                    <p>Dear {application.User.FirstName},</p>
                    <p>We are pleased to invite you for an interview regarding your application for the research program: {application.Title}.</p>
                    <p>Our team has reviewed your application and would like to discuss your qualifications and experience further.</p>
                    <p>Please respond to this email to schedule your interview.</p>
                    <br/>
                    <p>Best regards,<br/>CSIRO Research Team</p>
                </body>
                </html>";

            try
            {
                _logger.LogInformation("Attempting to send interview invitation to {Email}", application.User.Email);

                if (_environment.IsDevelopment())
                {
                    _logger.LogInformation("Development environment detected. Simulating email send.");
                    await SimulateEmailSendAsync(application.User.Email, emailSubject, emailBody);
                    _logger.LogInformation("Email simulation completed successfully");
                    return;
                }

                // Production email sending logic would go here
                throw new NotImplementedException("Production email sending is not yet implemented");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending interview invitation to {Email}", application.User.Email);
                throw new Exception($"Failed to send email: {ex.Message}", ex);
            }
        }

        private async Task SimulateEmailSendAsync(string toEmail, string subject, string body)
        {
            // Create a directory to store simulated emails if it doesn't exist
            var emailDir = Path.Combine(_environment.ContentRootPath, "EmailSimulation");
            Directory.CreateDirectory(emailDir);

            // Create a file with the email content
            var timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var fileName = $"email_{timestamp}.html";
            var filePath = Path.Combine(emailDir, fileName);

            var emailContent = $@"
                <html>
                <head>
                    <style>
                        .email-metadata {{ background: #f0f0f0; padding: 10px; margin-bottom: 20px; }}
                    </style>
                </head>
                <body>
                    <div class='email-metadata'>
                        <p><strong>To:</strong> {toEmail}</p>
                        <p><strong>Subject:</strong> {subject}</p>
                        <p><strong>Date:</strong> {DateTime.Now}</p>
                    </div>
                    <div class='email-body'>
                        {body}
                    </div>
                </body>
                </html>";

            await File.WriteAllTextAsync(filePath, emailContent);
            _logger.LogInformation("Simulated email saved to: {FilePath}", filePath);
        }
    }
}
