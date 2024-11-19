# CSIRO Application Portal

A modern web application for managing research candidate applications at CSIRO. Built with ASP.NET Core MVC and PostgreSQL.

## What it does

- **Application Submission**: Streamlined process for candidates to submit research applications
- **Status Tracking**: Real-time tracking of application progress
- **Program Discovery**: Browse available research opportunities
- **Secure Authentication**: Role-based access for applicants and administrators

## Technical Stack

- **Framework**: ASP.NET Core MVC (.NET 8.0)
- **Database**: PostgreSQL with Entity Framework Core
- **Authentication**: ASP.NET Core Identity
- **UI**: Bootstrap 5 with responsive design

## Key Features

- Modern, responsive interface
- Secure user authentication
- Role-based access control
- Application status tracking
- Document upload and management
- Email notifications
- Admin dashboard

## Requirements

- .NET 8.0 SDK
- PostgreSQL 15+
- Modern web browser

## Development

1. Clone the repository
2. Install dependencies:
   ```bash
   dotnet restore
   ```
3. Update database connection in `appsettings.json`
4. Run migrations:
   ```bash
   dotnet ef database update
   ```
5. Start the application:
   ```bash
   dotnet run
   ```

Access the application at `http://localhost:5002`