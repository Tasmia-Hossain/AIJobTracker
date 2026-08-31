# AIJobTracker

> **A focused job-search workspace for organizing applications, tracking progress, and analyzing job descriptions with AI.**

AIJobTracker is a full-stack **ASP.NET Core MVC** web application designed to help job seekers manage their job application pipeline from one place.

Users can securely create an account, track their own applications, search and filter opportunities, monitor application progress through a dashboard, and analyze job descriptions using an AI-powered job analyzer.

---

## Highlights

- **Secure authentication** with ASP.NET Core Identity
- **User-specific data** so each user can access and manage only their own applications
- **Complete CRUD workflow** for job applications
- **Search and status filtering** for efficient application management
- **Dashboard overview** with application statistics
- **AI Job Analyzer** powered by Google Gemini
- **Markdown-formatted AI reports** rendered using Markdig
- **Responsive light and dark theme**
- **Server-side and client-side validation**
- **Responsive SaaS-style user interface**
- **SQL Server + Entity Framework Core** persistence

---

## Screenshots

### Home

The landing page introduces AIJobTracker and explains its core workflow.

<p align="center">
  <img src="screenshots/home-1.png" width="49%" alt="AIJobTracker home page - hero section">
  <img src="screenshots/home-2.png" width="49%" alt="AIJobTracker home page - features and workflow">
</p>

### Authentication

<p align="center">
  <img src="screenshots/login.png" width="49%" alt="AIJobTracker login page">
  <img src="screenshots/register.png" width="49%" alt="AIJobTracker registration page">
</p>

### Dashboard & Applications

<p align="center">
  <img src="screenshots/dashboard.png" width="49%" alt="AIJobTracker dashboard">
  <img src="screenshots/jobs-list.png" width="49%" alt="AIJobTracker job applications list">
</p>

### Add Application & AI Analyzer

<p align="center">
  <img src="screenshots/create-job.png" width="49%" alt="AIJobTracker add job application page">
  <img src="screenshots/ai-analyzer.png" width="49%" alt="AIJobTracker AI job analyzer">
</p>

---

## Features

### Authentication & Authorization

AIJobTracker uses **ASP.NET Core Identity** to provide secure user authentication and authorization.

The application supports:

- User registration
- User login
- Secure logout
- Protected application-management pages
- User-specific job data
- User-specific dashboard statistics
- Anti-forgery protection for POST requests

Each user's job applications are isolated from other users.

### Job Application Management

Users can:

- Add new job applications
- View application details
- Edit application information
- Delete applications with confirmation
- Track the current application status

Each job application contains:

- Job title
- Company
- Location
- Application status
- Applied date
- Associated user

### Search & Filtering

The applications page supports:

- Search by job title
- Search by company
- Search by location
- Filter by application status
- Combined search and filtering

These features make it easier to find and manage specific applications.

### Dashboard

The dashboard provides an overview of the authenticated user's application pipeline.

It includes statistics for:

- Total applications
- Saved applications
- Applied applications
- Interview applications
- Offer applications
- Rejected applications
- Withdrawn applications

All dashboard statistics are calculated from the current user's own applications.

### AI Job Analyzer

The AI Job Analyzer accepts a job description and uses **Google Gemini** to generate a structured analysis.

The analysis can provide information such as:

- Job description summary
- Required technical skills
- Important keywords
- Experience requirements
- Candidate-focused recommendations

AI-generated Markdown content is rendered as formatted HTML using **Markdig**.

---

## Tech Stack

| Category | Technology |
|---|---|
| Language | C# |
| Framework | ASP.NET Core MVC |
| Runtime | .NET 10 |
| Authentication | ASP.NET Core Identity |
| ORM | Entity Framework Core |
| Database | SQL Server |
| Data Access | Entity Framework Core Code First |
| Views | Razor Views |
| Frontend | HTML, CSS, JavaScript, Bootstrap |
| AI | Google Gemini |
| Markdown Rendering | Markdig |
| Version Control | Git & GitHub |

---

## Architecture & Project Structure

The project follows the **ASP.NET Core MVC** architecture.

```text
AIJobTracker/
├── Controllers/
│   ├── AccountController.cs
│   ├── DashboardController.cs
│   ├── HomeController.cs
│   ├── JobsController.cs
│   └── AIController.cs
│
├── Data/
│   └── ApplicationDbContext.cs
│
├── Migrations/
│
├── Models/
│   ├── ApplicationUser.cs
│   ├── DashboardViewModel.cs
│   └── Job.cs
│
├── Views/
│   ├── Account/
│   ├── Dashboard/
│   ├── Home/
│   ├── Jobs/
│   ├── AI/
│   └── Shared/
│
├── wwwroot/
│   ├── css/
│   └── js/
│
├── Program.cs
├── appsettings.json
└── AIJobTracker.csproj
```

---

## Application Workflow

```text
Register
   ↓
Login
   ↓
Dashboard
   ↓
Add Job Application
   ↓
Track Application Status
   ↓
Search / Filter Applications
   ↓
View / Edit / Delete
   ↓
Analyze Job Description with AI
   ↓
Prepare for the Next Opportunity
```

---

## Database

AIJobTracker uses **SQL Server** with **Entity Framework Core** following a **Code First** approach.

### Main Entities

- `ApplicationUser` — represents an authenticated application user
- `Job` — represents an individual job application

### Main Components

- `DashboardViewModel` — a view model (not a database entity) that provides application statistics for the dashboard

Each `Job` is associated with its owning `ApplicationUser`.

Entity Framework Core migrations are used to manage database schema changes.

---

## Application Statuses

The tracker supports the following application stages:

- **Saved**
- **Applied**
- **Interview**
- **Offer**
- **Rejected**
- **Withdrawn**

These statuses allow users to track the progress of each application throughout the hiring process.

---

## CRUD Operations

| Operation | Description |
|---|---|
| Create | Add a new job application |
| Read | View personal applications and application details |
| Update | Edit existing application information |
| Delete | Remove an application with confirmation |

---

## Validation & Security

The application includes several validation and security mechanisms:

- ASP.NET Core Identity authentication
- Authorization for protected routes
- User-specific data access
- Model validation using C# Data Annotations
- Required-field validation
- Server-side validation
- Client-side validation
- Anti-forgery validation on POST operations
- Separation of application data by authenticated user

### Security Note

API keys, database credentials, and other sensitive information should never be committed to source control.

For local development, sensitive configuration should be stored securely using appropriate mechanisms such as **User Secrets** or environment variables.

---

## Getting Started

### Prerequisites

Make sure the following are installed:

- [.NET SDK](https://dotnet.microsoft.com/download)
- Visual Studio 2022 or later, or another compatible .NET IDE
- SQL Server or SQL Server LocalDB
- Git
- Entity Framework Core CLI tools

If the Entity Framework Core CLI tools are not installed:

```bash
dotnet tool install --global dotnet-ef
```

### 1. Clone the Repository

```bash
git clone https://github.com/Tasmia-Hossain/AIJobTracker.git
```

### 2. Navigate to the Project

```bash
cd AIJobTracker
```

### 3. Restore Dependencies

```bash
dotnet restore
```

### 4. Configure the Database

Configure the SQL Server or SQL Server LocalDB connection string in your local development configuration.

Do not commit database credentials or other sensitive information to GitHub.

### 5. Configure Gemini API

Configure the required Google Gemini API credentials using a secure local configuration mechanism such as User Secrets or environment variables.

Do not commit API keys to the repository.

### 6. Apply Entity Framework Migrations

```bash
dotnet ef database update
```

### 7. Run the Application

```bash
dotnet run
```

Then open the local URL displayed in the terminal.

---

## Build & Verification

To build the project:

```bash
dotnet build
```

The project currently builds successfully with **0 errors**.

---

## Learning Goals

This project was developed as a practical learning and portfolio project to strengthen skills in:

- C#
- ASP.NET Core MVC
- ASP.NET Core Identity
- Entity Framework Core
- SQL Server
- MVC architecture
- CRUD application development
- Authentication & authorization
- User-specific data handling
- Model binding
- Data validation
- Razor Views
- Bootstrap
- JavaScript
- Git & GitHub
- AI API integration
- Markdown rendering

---

## Future Improvements

Potential future improvements include:

- Password reset and email verification
- Job application notes
- Interview scheduling
- Application reminders
- Resume and document attachments
- Job posting URLs
- Salary information
- Company information
- Pagination
- Advanced sorting
- REST API
- AI-powered job recommendations
- AI-assisted resume and job matching
- Cloud deployment

---

## Author

**Tasmia Hossain**

Computer Science & Engineering

GitHub: [Tasmia-Hossain](https://github.com/Tasmia-Hossain)

---

## License

This project was created for **learning and portfolio purposes**.