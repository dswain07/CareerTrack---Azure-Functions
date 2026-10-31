# CareerTrack — Job Application Management System

**CSCE 547: Windows Programming | Minor Project 5**

CareerTrack is a job application management system built with C#, ASP.NET Core Web API, Entity Framework Core, and SQLite. It demonstrates a layered application architecture, RESTful CRUD operations, business-rule validation, and a browser-based client built with HTML, CSS, and JavaScript.

## Project overview

CareerTrack organizes employer information, job applications, and interview records in one place. The application uses an existing SQLite database from the earlier CareerTrack project and exposes the data through an ASP.NET Core Web API.

The current browser interface provides:

- A dashboard showing counts of companies, job applications, and interviews, plus recent applications.
- Lists, search, and individual detail views for companies, job applications, and interviews.
- Create, view, update, and delete operations for companies.
- Business-rule validation through the API.

The API additionally supports full CRUD for job applications and interviews, even though the current browser interface exposes these entities primarily as read-only views.

## Technology stack

| Component | Technology |
| --- | --- |
| Language | C# |
| Framework | .NET 10 / ASP.NET Core Web API |
| Data access | Entity Framework Core |
| Database | SQLite |
| Client | HTML, CSS, JavaScript (`fetch`) |
| Development environment | Visual Studio on Windows |
| API testing | Visual Studio `.http` requests |

## Architecture

CareerTrack separates responsibilities into three main application layers, with a separate browser client:

```text
Browser Client (wwwroot: HTML / CSS / JavaScript)
                      |
                 HTTP / JSON
                      |
CareerTrack.Api       |  REST endpoints and service models
                      v
CareerTrack.Business  |  Business services and validation rules
                      v
CareerTrack.Data      |  Repositories, EF Core models, DbContext
                      v
                 SQLite Database
```

### Solution projects

- **CareerTrack.Api** — ASP.NET Core controllers, client-facing service models, application startup, and static web client in `wwwroot`.
- **CareerTrack.Business** — `CompanyService`, `JobApplicationService`, and `InterviewService` implement application logic and validation.
- **CareerTrack.Data** — EF Core entity models, `CareerTrackDbContext`, repositories, and migrations.
- **CareerTrack.Client** — A separate C# console HTTP client used for API demonstrations and testing. The browser interface is the primary user-facing client.

The API controllers call business services, which call data repositories. Database queries are not performed directly in the controllers.

## Database design

The SQLite database contains five tables:

| Table | Description |
| --- | --- |
| `Companies` | Employer name, industry, location, and website |
| `ApplicationStatuses` | Lookup values for application progress |
| `JobApplications` | Position, application date, company, status, location, salary, URL, and notes |
| `Interviews` | Interview type, date, location, outcome, notes, and associated application |
| `Contacts` | Recruiter/contact details associated with companies |

Relationships include:

- `JobApplications.CompanyId` → `Companies.Id`
- `JobApplications.ApplicationStatusId` → `ApplicationStatuses.Id`
- `Interviews.JobApplicationId` → `JobApplications.Id`
- `Contacts.CompanyId` → `Companies.Id`

The original EF Core migration configures cascading foreign-key deletes. The business layer adds a rule that prevents deleting a company with associated job applications through the API.

Sample seed data includes **10 companies, 7 application statuses, 15 job applications, 5 interviews, and 3 contacts**. The database may contain different counts after interactive testing.

The `Scripts/` folder contains:

- `DDL.sql` — Creates the database tables, constraints, and indexes.
- `DML.sql` — Inserts the sample records into a freshly created schema.

**Do not run these initialization scripts against a populated CareerTrack database.** Use a new, empty SQLite database when testing them.

## REST API endpoints

The API exposes the following operations:

| Entity | Method | Endpoint | Purpose |
| --- | --- | --- | --- |
| Companies | GET | `/api/companies` | List companies |
| Companies | GET | `/api/companies/{id}` | Retrieve company by ID |
| Companies | POST | `/api/companies` | Create company |
| Companies | PUT | `/api/companies/{id}` | Update company |
| Companies | DELETE | `/api/companies/{id}` | Delete company |
| Job Applications | GET | `/api/jobapplications` | List applications |
| Job Applications | GET | `/api/jobapplications/{id}` | Retrieve application by ID |
| Job Applications | POST | `/api/jobapplications` | Create application |
| Job Applications | PUT | `/api/jobapplications/{id}` | Update application |
| Job Applications | DELETE | `/api/jobapplications/{id}` | Delete application |
| Interviews | GET | `/api/interviews` | List interviews |
| Interviews | GET | `/api/interviews/{id}` | Retrieve interview by ID |
| Interviews | POST | `/api/interviews` | Create interview |
| Interviews | PUT | `/api/interviews/{id}` | Update interview |
| Interviews | DELETE | `/api/interviews/{id}` | Delete interview |

API responses use standard HTTP status codes such as `200 OK`, `201 Created`, `204 No Content`, `400 Bad Request`, `404 Not Found`, and `409 Conflict`.

### Service models

`CompanyModel`, `JobApplicationModel`, and `InterviewModel` define the data exchanged with API clients. They expose the fields needed by the client while avoiding unnecessary navigation properties and nested EF Core entity graphs.

## Graduate-level business rules

The business layer enforces five rules:

1. **Prevent duplicate company names.** Names are compared after trimming surrounding whitespace and ignoring case. Applies to company creation and updates.
2. **Prevent deletion of companies with job applications.** The API rejects deletion when related job applications exist.
3. **Reject negative salaries.** A job application's salary may be unknown (`null`), but a negative value is invalid. Applies to creation and updates.
4. **Validate company and application status references.** A job application must reference an existing company and application status. Applies to creation and updates.
5. **Validate interview application references.** An interview must reference an existing job application. Applies to creation and updates.

These checks run in business services before calling the repository to save changes.

## Running the application

### Prerequisites

- Windows with Visual Studio (ASP.NET and web development workload)
- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- Git (for cloning the repository)
- SQLite database file or a new database initialized from the provided scripts/migrations

### 1. Clone and open the solution

```bash
git clone <REPOSITORY-URL>
cd <REPOSITORY-DIRECTORY>
```

Replace the placeholders with the actual GitHub repository URL and directory. Open the solution (`.sln` or `.slnx`) in Visual Studio.

### 2. Configure SQLite

The API reads its connection string from `CareerTrack.Api/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=careertrack.db"
  }
}
```

`Data Source=careertrack.db` is a **relative path**. SQLite resolves it relative to the application's working directory. Ensure that the expected database file is available there, or replace the value with a path to the database file.

For a new database, create the schema and seed data using the supplied SQL scripts in a fresh SQLite database, or use the project's EF Core migration workflow. Avoid running initialization scripts against the existing populated database.

### 3. Build the solution

In Visual Studio, select **Build → Build Solution**. Alternatively, from the solution directory:

```bash
dotnet build
```

### 4. Start the API and web interface

Set **CareerTrack.Api** as the startup project and run it. In the tested local configuration, the API listens on:

- `http://localhost:5043`
- `https://localhost:7220`

Open the browser client at:

```text
http://localhost:5043/
```

The API project serves the browser files from `wwwroot` using `UseDefaultFiles()` and `UseStaticFiles()`.

**Local development note:** HTTPS redirection was temporarily disabled during testing because the local ASP.NET Core development certificate was not trusted. The HTTP URL is suitable for local coursework testing only. Restore trusted HTTPS and appropriate access controls before network deployment.

### 5. Optional console client

Keep the API running, then run the separate console client:

```bash
dotnet run --project CareerTrack.Client
```

The console client communicates with the API over HTTP and demonstrates GET operations. Ensure its configured base URL matches the API's actual listening address.

## Testing and verification

Testing was performed using the browser client, a separate console client, and Visual Studio HTTP requests.

### CRUD verification

- GET all and GET by ID were verified for Companies, JobApplications, and Interviews.
- Company create, view, update, and delete were exercised through the browser interface.
- JobApplication create, update, and delete were exercised through HTTP requests using temporary records.
- Interview create, update, and delete were exercised through HTTP requests using temporary records.
- After deletion, GET by ID returned `404 Not Found` for the removed temporary records.

### Business-rule verification

All six negative test cases returned the expected response:

| Test | Expected result | Observed result |
| --- | --- | --- |
| Duplicate company name | 409 Conflict | Passed |
| Delete company with applications | 409 Conflict | Passed |
| Negative salary | 400 Bad Request | Passed |
| Nonexistent company reference | 400 Bad Request | Passed |
| Nonexistent application status | 400 Bad Request | Passed |
| Nonexistent interview application reference | 400 Bad Request | Passed |

The six tests cover the five business rules because rule 4 has two separate foreign-key checks.

The `CareerTrackTests.http` file contains requests for API testing. Tests that create or delete records should use temporary data rather than modifying original seed records.

## Screenshots

Add screenshots to a `Screenshots/` directory for submission, such as:

- Dashboard with record counts
- Companies page and Company Add/Edit operation
- Job Applications page and application details
- Interviews page and interview details
- Successful API POST/PUT/DELETE responses
- Business-rule rejection responses (`400` and `409`)

Screenshots are not embedded here until they have been captured and added to the repository.

## Known limitations and future work

The current browser UI provides full Company CRUD but mainly read-only views for JobApplications and Interviews. Potential enhancements include:

- Browser forms for creating and editing applications and interviews
- Kanban tracking by application status
- Follow-up reminders and interview scheduling
- Analytics and application conversion metrics
- Resume and cover-letter tracking
- Authentication, authorization, and secure deployment
- Database backup and improved error handling

## Coursework scope

CareerTrack demonstrates a functioning three-tier service architecture, client-facing service models, REST API CRUD endpoints, and five graduate-level business rules. The current version is a local coursework prototype, not a production-ready multi-user service.
