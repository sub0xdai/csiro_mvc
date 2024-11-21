# CSIRO Research Application Portal

## Overview
A web application built for CSIRO to streamline the management of research position applications in Machine Learning and Data Science, with a focus on COVID-19 vaccination causality research in Australia.

## Features

### For Applicants
- User registration and profile management
- Application submission for research positions
- Educational background and experience documentation
- Document upload capability (CV, cover letter)
- Real-time application status tracking
- Automated email notifications

### For Administrators
- Comprehensive application review dashboard
- Advanced sorting and filtering capabilities
- Automated candidate shortlisting
- Interview scheduling management
- Email communication system
- Application status management

## Technical Stack
- **Backend**: ASP.NET Core MVC (.NET 8.0)
- **Database**: PostgreSQL
- **Authentication**: ASP.NET Core Identity
- **Frontend**: Bootstrap 5, jQuery
- **Email**: Development simulation with production SMTP support

## Getting Started

### Prerequisites
- .NET 8.0 SDK
- PostgreSQL 15 or higher
- Visual Studio 2022 or VS Code

### Installation
1. Clone the repository
```bash
git clone [repository-url]
cd csiro_mvc
```

2. Update database connection string in `appsettings.json`
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Database=csiro_mvc;Username=your_username;Password=your_password"
  }
}
```

3. Apply database migrations
```bash
dotnet ef database update
```

### Running the Application
1. Start the application:
```bash
dotnet run --launch-profile http
```

2. Access the application at:
- http://localhost:5002

## Email Testing in Development

During development, the application simulates email sending by saving email content as HTML files:

1. All emails are saved in the `EmailSimulation` directory
2. Files are named with timestamps (e.g., `email_20240101_120000.html`)
3. Each file contains:
   - Email metadata (recipient, subject, timestamp)
   - Formatted email content
   - Styling for visualization

## Production Deployment

For production deployment:
1. Update email configuration in `appsettings.Production.json`
2. Set environment variable: `ASPNETCORE_ENVIRONMENT=Production`
3. Configure your SMTP server details

## Security Features
- Role-based authorization
- Secure password hashing
- Input validation and sanitization
- CSRF protection
- XSS prevention
- SQL injection protection

## Contributing
This project is part of a group assessment. All contributions should be:
- Properly documented
- Tested thoroughly
- Reviewed by team members
- Compliant with the existing code style

## License
[License details to be added]