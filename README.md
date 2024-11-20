# CSIRO Research Application Portal

## Project Overview
A web application for CSIRO to manage applications for research positions in Machine Learning and Data Science, focusing on COVID-19 vaccination causality research in Australia.

## Core Requirements Analysis

### User Authentication & Authorization
- [x] User Registration System
- [x] Role-based Authorization (Admin/Applicant)
- [x] Secure Password Encryption
- [x] Session Management

### Applicant Features
1. Profile Management
   - [ ] Contact Details
   - [ ] Educational Background
   - [ ] Profile Editing Capability

2. Application Submission
   - [ ] Course Selection (Dropdown)
     * Master of Data Science
     * Master of Artificial Intelligence
     * Master of Information Technology
     * Master of Science (Statistics)
   - [ ] GPA Entry (Server-side validation)
     * Must be ≥ 3.0
     * Double/Float validation
   - [ ] University Selection
     * Limited to top 100 global universities
   - [ ] Cover Letter Submission
   - [ ] CV Upload (Optional)

### Administrator Features
1. Application Management
   - [ ] View All Applications
   - [ ] Sort Applications by GPA
   - [ ] Search Functionality
   - [ ] Configurable GPA Cutoff

2. Interview Process
   - [ ] Automated Email System
   - [ ] Template-based Messages
   - [ ] Top 10 Candidate Selection

### Security Requirements
1. Data Protection
   - [ ] Password Encryption
   - [ ] Input Sanitization
   - [ ] Server-side Validation

2. Access Control
   - [ ] Role-based Access
   - [ ] Session Management
   - [ ] Secure Routes

### Technical Stack
- Backend: ASP.NET Core MVC (C#)
- Database: PostgreSQL
- Authentication: ASP.NET Core Identity
- Frontend: Bootstrap, jQuery
- Email: SMTP Integration

## Project Deliverables
1. [ ] UML & ER Diagrams Documentation
2. [x] MVC Implementation
3. [ ] Test Cases Documentation
4. [ ] Database SQL File

## Development Progress
- [x] Basic Project Setup
- [x] Database Configuration
- [x] User Authentication System
- [ ] Application Form
- [ ] Admin Dashboard
- [ ] Email Integration
- [ ] Testing & Documentation

## Testing Requirements
- [ ] Unit Tests
- [ ] Integration Tests
- [ ] User Acceptance Testing
- [ ] Security Testing

## Security Measures
- [x] Password Encryption
- [ ] Input Validation
- [ ] XSS Prevention
- [ ] CSRF Protection
- [ ] SQL Injection Prevention

## Installation & Setup
[To be added]

## Contributing
This is a group assessment project (2+ members). Each member's contributions should be clearly documented and demonstrable.

## License
[To be added]