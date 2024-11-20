# 11.18
- Initialized ASP.NET Core MVC project (.NET 8.0)
- Setup architecture and file/folder structure based on the requirements and provided diagrams
- Enabled vim mode on vscode
- Setup PorstgreSQL databse for Linux environment 
- Configured project dependancies and NuGet packages
    - Entity framework
    - Identity Framework
    - PostgreSQL 
- Implemented data models
- Setup ApplicationDbContext

_Today I kicked off an ASP.NET Core MVC project using .NET 8.0. I set up the file and folder structure following the provided diagrams and requirements. I enabled Vim mode in VS Code, configured PostgreSQL for Linux, and installed dependencies like Entity Framework, Identity Framework, and PostgreSQL via NuGet. I also created the data models and set up the ApplicationDbContext. Next time, I'll focus on adding a repository pattern, building a service layer, setting up authentication, designing views, and cleaning up the project with a .gitignore file._

## Next session
- Implement repository pattern
- Create service layer
- Set up authentication controllers
- Design and implement views
- Add .gitignore 

# 11.19  
- Redesigned UI with a modern, responsive layout.  
- Added Bootstrap Icons for visual elements.  
- Fixed layout template and styles.  
- Created a card-based home page with hover effects and animations.  
- Set up basic navigation structure.  
- Implemented ASP.NET Core Identity for authentication.  
- Extended User model with required fields.  
- Added Application model with relationships.  
- Fixed PostgreSQL timestamp issues in migrations.  
- Enhanced database configuration and model validation.  
- Created custom ViewModels and responsive auth pages.  
- Improved routing, navigation, and error handling in auth flows.  
- Implemented repository pattern with async operations:  
  - Created `IGenericRepository` interface and `GenericRepository` base class.  
  - Added specialized `ApplicationRepository`.  
  - Fixed async/await patterns.  
- Enhanced service layer:  
  - Added `ApplicationService` for CRUD operations.  
  - Implemented `ApplicationSettingsService`.  
  - Resolved nullability and type safety issues.  
- Added new views and features:  
  - Research Programs listing page.  
  - Application Status tracking view.  
  - Enhanced navigation with card-based UI.  
- Fixed technical issues:  
  - Resolved async method warnings and property mismatches.  
  - Corrected `ApplicationStatus` enum usage.  
  - Improved error handling and null checks.  

## Completed Features  
- User registration and login/logout with secure password hashing.  
- Custom User model and PostgreSQL database integration.  
- Modern, responsive UI with Bootstrap-based design.  
- Navigation and routing.  
- Basic error handling.  
- Repository pattern and async/await integration.  
- Research Programs browsing.  
- Application Status tracking.  
- Enhanced error handling and type safety.  


_Today, I polished the UI with responsive design, Bootstrap icons, and animations, fixed layout issues, and built a card-based home page. I implemented authentication using ASP.NET Core Identity, extended the User model, and resolved PostgreSQL timestamp issues. I also added custom ViewModels, modernized auth pages, and improved routing, error handling, and database setup. Key features now include user registration, login/logout, a secure database integration, and a sleek Bootstrap-based design. Next, I'll focus on user profiles, role-based auth, forms, an admin dashboard, file uploads, and email notifications while addressing logging, testing, and security improvements._

## Next Session
- Implement user profile completion flow.  
- Add role-based authorization.  
- Create application submission form.  
- Set up an admin dashboard.  
- Add file upload functionality for CVs/documents.  
- Implement email notifications and input validation.  
- Create an application review interface.  

## Technical Debt  
- Add comprehensive logging and unit tests.  
- Optimize database queries and add caching.  
- Implement proper CI/CD pipeline and API documentation.  

## Security Considerations  
- Add rate limiting, CSRF protection, and CORS policies.  
- Implement audit logging and session management.  
- Ensure proper SSL/TLS setup.  


# 11.20

## Session 2024-11-20

### Goals Review
Original goals for this session:
- Implement user profile completion flow
- Add role-based authorization
- Create application submission form
- Set up an admin dashboard
- Add file upload functionality for CVs/documents
- Implement email notifications and input validation
- Create an application review interface

### What Was Actually Done
1. **Identity Model Cleanup**
   - Resolved conflicts between duplicate user models (User.cs and Users.cs)
   - Consolidated user identity model to use ApplicationUser consistently throughout the application
   - Cleaned up redundant model definitions to prevent compilation errors

### Technical Debt Addressed
- Removed duplicate user model definitions that could have caused issues with Identity framework
- Ensured consistent use of ApplicationUser model across the application

### Challenges Encountered
- Multiple user model definitions causing compilation errors
- Identity configuration issues that needed to be resolved

### Next Steps
Original goals remain to be implemented:
- [x] Implement user profile completion flow
- [ ] Add role-based authorization
- [ ] Set up an admin dashboard
- [ ] Add file upload functionality for CVs/documents
- [ ] Implement email notifications and input validation
- [ ] Create an application review interface

## Session 2024-11-21

### What Was Done
1. **Application Submission Form Implementation**
   - Created and integrated the `ApplicationForm` model with necessary fields
   - Implemented the Apply view with form fields and validation
   - Added file upload functionality for CV documents
   - Updated the ApplicationController with Apply actions
   - Fixed package version conflicts (EF Core InMemory)

2. **Repository and Service Layer Enhancements**
   - Updated ApplicationRepository with improved search functionality
   - Added string comparison fixes using EF.Functions.Like
   - Implemented proper file handling for CV uploads

### Features Completed
- [x] Create application submission form
- [x] Add file upload functionality for CVs
- [x] Implement input validation for application form

### Technical Improvements
- Fixed package version conflicts by aligning all EF Core packages to version 8.0.0
- Enhanced search functionality with case-insensitive comparisons
- Improved file upload handling with proper directory creation

### Next Steps
Remaining goals to implement:
- [ ] Implement user profile completion flow
- [ ] Add role-based authorization
- [ ] Set up an admin dashboard
- [ ] Implement email notifications
- [ ] Create an application review interface

### Technical Debt
- Add unit tests for new application submission functionality
- Implement proper error handling for file uploads
- Add input sanitization for file uploads

### Authentication and Profile System Updates (11.20)
1. **User Profile Implementation**
   - Created ProfileViewModel with validation attributes
   - Implemented ProfileController with view/update capabilities
   - Added profile completion form with all required fields
   - Integrated profile access in navigation menu

2. **Authentication System Improvements**
   - Separated login and registration functionality
   - Created dedicated ViewModels for Login and Register
   - Implemented proper session persistence
   - Added "Remember Me" functionality
   - Enhanced error handling and validation
   - Fixed authentication token persistence issues

3. **UI/UX Enhancements**
   - Added clear validation messages
   - Improved error visibility
   - Created separate views for Login and Register
   - Added navigation between authentication pages
   - Enhanced form layout and styling

### Technical Debt
- Add comprehensive logging and unit tests.  
- Optimize database queries and add caching.  
- Implement proper CI/CD pipeline and API documentation.  
- Add unit tests for new application submission functionality
- Implement proper error handling for file uploads
- Add input sanitization for file uploads

## Role-Based Authorization Fix - 2024-11-22

### Issues Fixed
1. Role Management visibility issue for admin users
2. Role claims not being properly loaded during sign-in

### Changes Made
1. **AccountController.cs**
   - Enhanced login process to properly handle role-based authentication
   - Added explicit role claim loading during sign-in
   - Improved logging for role assignments and login attempts
   - Added sign-out before sign-in to ensure clean role claims

2. **Database Reset and Initialization**
   - Reset database to ensure clean state
   - Verified proper role seeding (Admin, Researcher, Applicant)
   - Confirmed admin user creation with correct role assignment

### Current Status
- Role-based authorization working as expected
- Admin users can access Role Management section
- Default roles assigned correctly:
  - First user: Admin role
  - Subsequent users: Applicant role
- Role claims properly loaded during authentication

### Next Steps
- Continue testing with new user registrations
- Verify role assignment for subsequent users
- Test role management functionality
