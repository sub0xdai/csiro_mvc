namespace csiro_mvc.Models
{
    public class ApplicationSettings
    {
        public double GPACutoff { get; set; } = 3.0;
        public int TopCandidatesCount { get; set; } = 10;
        public string EmailSenderName { get; set; } = "CSIRO HR";
        public string EmailTemplate { get; set; } = @"
Dear {FirstName},

We are pleased to inform you that your application for the CSIRO research position has been shortlisted. We would like to invite you for an interview to discuss your qualifications and potential contribution to our research on COVID-19 vaccination causality rates in Australia.

Please reply to this email with your preferred interview time slots for the next week.

Best regards,
{SenderName}
CSIRO Research Team
";
    }
}
