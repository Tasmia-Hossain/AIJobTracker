# AIJobTracker

AIJobTracker is a web-based job application tracking system built with ASP.NET Core MVC.

It helps users organize, track, search, filter, and manage their job applications from a single dashboard.

## Features

- Add new job applications
- Edit job applications
- Delete job applications with confirmation
- View job application details
- Search job applications
- Filter applications by status
- Dashboard with application statistics
- Form validation
- SQL Server database integration
- Entity Framework Core migrations
- Responsive Bootstrap-based UI

## Tech Stack

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- Razor Views
- Bootstrap
- HTML
- CSS
- JavaScript
- Git & GitHub

## Project Structure

```text
AIJobTracker/
├── Controllers/
├── Data/
├── Migrations/
├── Models/
├── Views/
├── wwwroot/
├── Program.cs
├── appsettings.json
└── AIJobTracker.csproj

## Application Workflow

The application follows a simple CRUD-based workflow:

1. Add a new job application
2. Store the application in the SQL Server database
3. View all tracked applications
4. Search applications
5. Filter applications by status
6. View detailed information about an application
7. Edit application information
8. Delete an application with confirmation
9. Monitor application statistics through the dashboard

## Job Application Information

Each job application contains:

- Job Title
- Company
- Location
- Application Status
- Applied Date

Supported application statuses:

- Saved
- Applied
- Interview
- Offer
- Rejected
- Withdrawn

## Database

The project uses:

- SQL Server
- Entity Framework Core
- Code First approach
- Entity Framework Core Migrations

The `Job` model is mapped to the `Jobs` table in the database.

## CRUD Operations

| Operation | Description |
|---|---|
| Create | Add a new job application |
| Read | View applications and application details |
| Update | Edit existing application information |
| Delete | Remove an application with confirmation |

## Search & Filtering

The application provides:

- Job search functionality
- Status-based filtering
- Combined search and filtering

These features allow users to quickly find specific applications.

## Dashboard

The dashboard provides an overview of job applications, including:

- Total Jobs
- Saved Jobs
- Applied Jobs
- Interview Jobs
- Offer Jobs
- Rejected Jobs
- Withdrawn Jobs

This gives users a quick overview of their job application progress.

## Validation

Form validation is implemented to ensure that required job information is provided before an application is submitted.

## Getting Started

### Prerequisites

Make sure you have the following installed:

- .NET SDK
- Visual Studio
- SQL Server or SQL Server LocalDB
- Git

### Clone the Repository

```bash
git clone https://github.com/Tasmia-Hossain/AIJobTracker.git
```

### Navigate to the Project

```bash
cd AIJobTracker
```

### Restore Dependencies

```bash
dotnet restore
```

### Apply Database Migration

```bash
dotnet ef database update
```

### Run the Application

```bash
dotnet run
```

Then open the local URL shown by the application.

## Screenshots

Screenshots of the application can be added here.

Example:

```text
screenshots/
├── dashboard.png
├── jobs-list.png
├── create-job.png
└── job-details.png
```

## Future Improvements

Possible future improvements include:

- User authentication and authorization
- User-specific job tracking
- Job application notes
- Interview scheduling
- Job application reminders
- Resume attachment
- Job posting URL
- Salary information
- Company information
- Pagination
- Sorting
- Improved dashboard charts
- REST API
- Cloud deployment

## Learning Goals

This project was built as a practical learning project to strengthen skills in:

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server
- MVC architecture
- CRUD operations
- Model binding
- Form validation
- Razor Views
- Git & GitHub

## Author

**Tasmia Hossain**

Computer Science & Engineering

GitHub: [Tasmia-Hossain](https://github.com/Tasmia-Hossain)

## License

This project is created for learning and portfolio purposes.