# 11.18

### Current Status
- Project initialization phase
- Basic architecture and environment setup complete
- Database configuration in progress

### Next Steps
- Implement repository pattern
- Create service layer
- Set up authentication controllers
- Design and implement views
- Add .gitignore 

### Completed Tasks
- Initialized ASP.NET Core MVC project (.NET 8.0)
- Setup architecture and file/folder structure based on requirements
- Enabled vim mode on vscode
- Setup PostgreSQL database for Linux environment 
- Configured project dependencies and NuGet packages:
    - Entity Framework
    - Identity Framework
    - PostgreSQL 
- Implemented data models
- Setup ApplicationDbContext

### Current Blockers
- None at this time

# 11.19

### Current Status
- Authentication system implemented
- Basic UI structure in place
- Repository pattern and service layer operational

### Next Steps
- Implement user profile completion flow
- Add role-based authorization
- Create application submission form
- Set up an admin dashboard
- Add file upload functionality for CVs/documents
- Implement email notifications and input validation
- Create an application review interface

### Completed Tasks
- Redesigned UI with modern, responsive layout
- Added Bootstrap Icons for visual elements
- Fixed layout template and styles
- Created card-based home page with hover effects
- Set up basic navigation structure
- Implemented ASP.NET Core Identity
- Extended User model with required fields
- Added Application model with relationships
- Fixed PostgreSQL timestamp issues
- Enhanced database configuration
- Created custom ViewModels
- Implemented repository pattern with async operations
- Enhanced service layer
- Added new views and features

### Current Blockers
- None at this time

### Technical Debt
- Add comprehensive logging and unit tests
- Optimize database queries and add caching
- Implement proper CI/CD pipeline and API documentation
- Ensure proper SSL/TLS setup
- Add rate limiting, CSRF protection, and CORS policies

# 11.20

### Current Status
- Identity system cleanup complete
- Application submission form implemented
- Profile system updates in progress
- Role-based authorization fixes implemented
- Profile completion flow working as expected

### Next Steps
- Complete user profile completion flow
- Add role-based authorization
- Set up admin dashboard
- Implement email notifications
- Create application review interface

### Completed Tasks
- Resolved conflicts between duplicate user models
- Consolidated user identity model
- Created and integrated ApplicationForm model
- Implemented Apply view with validation
- Added file upload functionality
- Updated ApplicationRepository
- Created ProfileViewModel
- Implemented ProfileController
- Added profile completion form
- Enhanced authentication system
- Improved UI/UX for forms and validation
- Fixed profile completion form submission
- Successfully tested profile completion flow
- Implemented user dashboard with profile information
- Added ResearchPrograms to database context
- Fixed role management visibility for admin users
- Enhanced login process with proper role claim handling
- Reset and verified database state with proper role seeding

### Current Blockers
- None at this time - all previous blockers resolved

### Technical Debt
- Add comprehensive logging and unit tests
- Optimize database queries and add caching
- Implement proper CI/CD pipeline and API documentation
- Add unit tests for new application submission functionality
- Implement proper error handling for file uploads
- Add input sanitization for file uploads

# 11.21

### Current Status
- Successfully implemented user authentication and profile completion flow
- Dashboard functionality is in place with basic user information display
- Research programs are seeded and accessible in the database

### Next Steps
- Continue testing with new user registrations
- Verify role assignment for subsequent users
- Test role management functionality
- Implement Dashboard/Home page for authenticated users
- Add user-specific content display
- Create intuitive navigation structure
- Make research program links clickable with detailed views
- Add more interactive features to the dashboard
- Consider adding application status tracking

### Completed Tasks
- Implemented initial dashboard with user profile information
- Set up research program data structure and seeding
- Added proper authentication checks in navigation
- Fixed null reference warnings throughout the application

### Current Blockers
- None at this time

#### Notes
- [x] Profile section has one too many steps, once you fill out profile it takes you to another page to add your phone number. It should be on the same page as the profile information and save data to the database.
- [x] View applications button on dashboard doesnt work
- [x] Add more interactive features to the dashboard
- [x] Add application status tracking
- [x] There is applications in nav, and my applications on the right side which is redundant 
- [x] Research position application page needs to be more functional,when I submit it should go to a page that says that I have submitted the application and then it should be saved to the database and then I should be able to see the status of my application in the dashboard
- On dashboard the recent research programs should take you to some page where you can see more details about the research program
- I need to have top 100 universities somewhere as per the requirements, probably should be in database and be a dropdown menu
- Profile should be not editable until profile completion, but there should be an edit button to allow users to update their profile
- Home page should be user friendly and be seperate to the dashboard view
