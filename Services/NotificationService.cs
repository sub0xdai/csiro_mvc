using System.Net.Mail;
using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using csiro_mvc.Models;
using Microsoft.AspNetCore.Hosting;

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
                _logger.LogInformation("Environment is Development: {IsDevelopment}", _environment.IsDevelopment());

                // Use MailHog for development, real SMTP for production
                var smtpServer = _environment.IsDevelopment() ? "localhost" : _configuration["EmailSettings:SmtpServer"];
                var smtpPort = _environment.IsDevelopment() ? 1025 : int.Parse(_configuration["EmailSettings:SmtpPort"] ?? "587");
                
                _logger.LogInformation("Using SMTP Server: {Server}, Port: {Port}", smtpServer, smtpPort);

                using var client = new SmtpClient
                {
                    Host = smtpServer,
                    Port = smtpPort,
                    EnableSsl = false, // Disable SSL for MailHog
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    UseDefaultCredentials = false // Don't use any credentials for MailHog
                };

                _logger.LogInformation("SMTP Client configured with SSL: {EnableSsl}, DeliveryMethod: {DeliveryMethod}", 
                    client.EnableSsl, client.DeliveryMethod);

                var fromEmail = _configuration["EmailSettings:FromEmail"] ?? "noreply@csiro.au";
                _logger.LogInformation("Sending from email: {FromEmail}", fromEmail);

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(fromEmail),
                    Subject = emailSubject,
                    Body = emailBody,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(application.User.Email);

                _logger.LogInformation("Attempting to send email...");
                await client.SendMailAsync(mailMessage);
                _logger.LogInformation("Interview invitation sent successfully to {Email}", application.User.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending interview invitation to {Email}. Error details: {Message}, Stack: {StackTrace}", 
                    application.User.Email, ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}
