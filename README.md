# CareerTrack — Azure Functions & Blob Storage Integration

**Course:** CSCE 547 — Windows Programming | Minor Project 6  
**Academic Level:** Graduate  
**Repository:** [https://github.com/dswain07/CareerTrack---Azure-Functions](https://github.com/dswain07/CareerTrack---Azure-Functions)  
**Development Environment:** Visual Studio 2026 | .NET 10 | Microsoft Azure  

---

## 1. Project Overview

**CareerTrack** is a full-stack job application tracking system built with C#, ASP.NET Core Web API, Entity Framework Core, SQLite, HTML5, CSS3, JavaScript, and Microsoft Azure. The application provides an interactive web interface to track companies, application pipelines, and interview stages.

This project extends Minor Project 5 by introducing three cloud-integrated capabilities:
1. **Application Priority Scoring:** Calculates a weighted application priority score (0–100) using application status, elapsed days since submission, and salary.
2. **Follow-Up Recommendations:** Determines recruiter outreach timing and actionable advice based on predefined rules across application lifecycle stages.
3. **Azure Blob Storage Reports (Graduate Requirement):** Generates structured JSON reports containing application and company metadata and uploads them securely into a private Azure Blob Storage container.

---

## 2. Technologies Used

| Technology | Purpose |
| :--- | :--- |
| **C# 14 / .NET 10** | Core backend language and application framework |
| **ASP.NET Core Web API** | RESTful service layer and static file hosting |
| **Entity Framework Core** | Object-Relational Mapping (ORM) for data persistence |
| **SQLite** | Embedded relational database |
| **HTML5 / CSS3** | Web UI structure and custom styles |
| **JavaScript (ES6+)** | Client-side asynchronous interaction via `fetch` API |
| **Azure Functions (.NET 10 Isolated)** | Serverless compute for scoring and recommendation logic |
| **Azure Blob Storage** | Private cloud storage for serialized JSON application reports |
| **Azure Functions Flex Consumption** | Linux-based serverless hosting tier |
| **HttpClient / IHttpClientFactory** | Inter-service HTTP communication between API and Azure |
| **Visual Studio 2026** | Integrated Development Environment (IDE) |
| **Git & GitHub** | Version control and remote repository hosting |

---

## 3. System Architecture

CareerTrack adheres to a layered architecture enforcing clean separation between presentation, API controllers, business domain logic, data persistence, and serverless cloud services.

```text
CareerTrack Web Interface
(HTML, CSS, JavaScript)
          │
          │ HTTP Requests (fetch)
          ▼
CareerTrack.Api
(ASP.NET Core Web API)
          │
          ▼
CareerTrack.Business
(Business Logic & HttpClient)
          │
          ├─────────────────────────────────────────┐
          │                                         │
          ▼                                         ▼
CareerTrack.Data                            Microsoft Azure
(Repositories & SQLite)                     │
                                            ├── Azure Functions (Flex Consumption)
                                            │   ├── CalculateApplicationScore
                                            │   └── GenerateFollowUpRecommendation
                                            │
                                            └── Azure Blob Storage
                                                └── careertrack-reports (Private)
```

### Request Flow Dynamics

#### Azure Functions Flow
1. The user selects an action in the CareerTrack web interface.
2. Client-side JavaScript sends an HTTP request to the CareerTrack API.
3. The API controller delegates the call to the corresponding business layer service.
4. The business layer uses `HttpClient` to invoke the deployed Azure Function over HTTPS.
5. The Azure Function processes the input parameters and returns a JSON payload.
6. The business layer and API pass the result back to the web interface.
> *Note: The client never directly calls Azure Functions; all traffic is routed through the API gateway.*

#### Blob Storage Flow
1. The user clicks **Generate Report** in the web interface.
2. The browser sends a `POST` request to `/api/jobapplications/{id}/upload-report`.
3. The API invokes `BlobReportService`.
4. `BlobReportService` queries SQLite via EF Core to fetch the application and associated company.
5. The service serializes the data into a JSON document.
6. The JSON payload is uploaded to the private Azure Blob Storage container (`careertrack-reports`).
7. Confirmation and the generated filename are returned to the client interface.

---

## 4. Project Structure

```text
CareerTrack.Api.ServiceLayer/
│
├── CareerTrack.Api/                      # ASP.NET Core Web API & Static Web Host
│   ├── Controllers/                      # REST API endpoints
│   ├── ServiceModels/                    # Request/Response DTOs
│   ├── wwwroot/                          # Web UI assets
│   │   ├── index.html                    # Dashboard interface
│   │   ├── app.js                        # Client logic & API calls
│   │   └── style.css                     # Custom styling
│   └── Program.cs                        # App initialization & DI configuration
│
├── CareerTrack.Business/                 # Business logic and external integrations
│   ├── Models/                           # Business domain models
│   │   ├── ApplicationScoreResult.cs
│   │   └── FollowUpRecommendationResult.cs
│   └── Services/                         # Domain and cloud service integrations
│       ├── CompanyService.cs
│       ├── JobApplicationService.cs
│       ├── InterviewService.cs
│       └── BlobReportService.cs          # Azure Blob Storage client service
│
├── CareerTrack.Data/                     # EF Core Data Context & Entities
│   ├── Data/                             # CareerTrackDbContext
│   └── Models/                           # Entity definitions
│
├── CareerTrack.Data.Repositories/        # Data Access Layer
│   └── Repositories/                     # Repository pattern implementations
│
├── CareerTrack.AzureFunctions/           # Serverless Microservices Project
│   ├── CalculateApplicationScore.cs      # Score calculation function
│   ├── GenerateFollowUpRecommendation.cs # Follow-up recommendation function
│   ├── Program.cs                        # Isolated worker setup
│   └── host.json                         # Functions runtime configuration
│
├── CareerTrack.Client/                   # Original MP5 Console Client
└── CareerTrack.Api.ServiceLayer.slnx     # Visual Studio Solution file
```

---

## 5. Application Features

### Baseline Management Features
- Display companies, job applications, and interview records.
- Retrieve individual records via REST API endpoints.
- Full CRUD operations for company records.
- Filter, search, and sort displayed records.
- Inspect detailed application records (status, salary, company, date applied, notes).
- Aggregate metrics displayed on a web dashboard.

### Feature 1: Application Priority Scoring
The `CalculateApplicationScore` Azure Function computes a deterministic priority score between 0 and 100 based on application status, elapsed time, and salary:

* **Endpoint:** `GET /api/CalculateApplicationScore`
* **Query Parameters:** `status` (string), `daysSinceApplied` (int), `salary` (decimal)

```json
// Example Request:
// GET /api/CalculateApplicationScore?status=Applied&daysSinceApplied=12&salary=120000

{
  "status": "Applied",
  "daysSinceApplied": 12,
  "salary": 120000,
  "statusScore": 15,
  "daysScore": 15,
  "salaryScore": 20,
  "totalScore": 50
}
```

### Feature 2: Follow-Up Recommendations
The `GenerateFollowUpRecommendation` Azure Function determines whether candidate outreach is warranted based on predefined stage criteria:

* **Endpoint:** `GET /api/GenerateFollowUpRecommendation`
* **Query Parameters:** `status` (string), `daysSinceApplied` (int)

```json
// Example Request:
// GET /api/GenerateFollowUpRecommendation?status=Applied&daysSinceApplied=10

{
  "status": "Applied",
  "daysSinceApplied": 10,
  "shouldFollowUp": true,
  "recommendation": "Follow up with the recruiter."
}
```

### Feature 3: Azure Blob Storage Reports (Graduate Requirement)
Automates report generation and cloud upload for individual applications:

* **Storage Account:** `careertrackmp6storage`
* **Container Name:** `careertrack-reports`
* **Container Access:** Private
* **Payload Format:** JSON
* **Target Region:** West US 2

```json
// Example Report Structure:
{
  "ApplicationId": 1,
  "JobTitle": "Software Engineer",
  "Company": "Microsoft",
  "ApplicationStatusId": 2,
  "DateApplied": "2026-10-01T00:00:00",
  "GeneratedAtUtc": "2026-11-08T21:14:13Z"
}
```

---

## 6. Azure Deployment

The serverless components and cloud storage resources were deployed using an **Azure for Students** subscription.

### Cloud Resource Specifications

| Component | Configuration |
| :--- | :--- |
| **Resource Group** | `CareerTrack-MP6-RG` |
| **Function App** | `careertrack-mp6-flex` |
| **Hosting Plan** | Flex Consumption |
| **Operating System** | Linux |
| **Target Runtime** | .NET 10 Isolated Worker |
| **Region** | West US 2 |
| **Instance Memory** | 512 MB |
| **Storage Account** | `careertrackmp6storage` (Standard LRS) |
| **Application Insights** | Disabled |

### Deployment Considerations & Resolution
- **Consumption Quota Workaround:** The standard Windows Consumption plan was unavailable due to a zero-quota limit on App Service instances (`Y1`) under the student subscription. Switching to the **Azure Functions Flex Consumption** hosting plan on Linux bypassed this restriction.
- **Regional Constraints:** Regional storage capacity restrictions in East US were resolved by provisioning both the storage account and the Function App within **West US 2**.

---

## 7. REST API Endpoints

### Core Entities API

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/companies` | Retrieve all companies |
| `GET` | `/api/companies/{id}` | Retrieve company by ID |
| `POST` | `/api/companies` | Create a company |
| `PUT` | `/api/companies/{id}` | Update a company |
| `DELETE` | `/api/companies/{id}` | Delete a company |
| `GET` | `/api/jobapplications` | Retrieve all applications |
| `GET` | `/api/jobapplications/{id}` | Retrieve application by ID |
| `POST` | `/api/jobapplications` | Create an application |
| `PUT` | `/api/jobapplications/{id}` | Update an application |
| `DELETE` | `/api/jobapplications/{id}` | Delete an application |
| `GET` | `/api/interviews` | Retrieve all interviews |
| `GET` | `/api/interviews/{id}` | Retrieve interview by ID |

### Minor Project 6 Cloud Integration Endpoints

| Method | Endpoint | Description |
| :--- | :--- | :--- |
| `GET` | `/api/jobapplications/application-score` | Proxies scoring request to Azure Function |
| `GET` | `/api/jobapplications/follow-up-recommendation` | Proxies follow-up request to Azure Function |
| `POST` | `/api/jobapplications/{id}/upload-report` | Generates and uploads JSON report to Blob Storage |

---

## 8. Local Setup & Execution Guide

### Prerequisites
- Windows 10 or Windows 11
- [.NET 10 SDK](https://dotnet.microsoft.com/)
- Visual Studio 2026 (with ASP.NET and Azure development workloads)
- Git
- Active Azure subscription with a provisioned Blob Storage account

### 1. Clone the Repository
```bash
git clone https://github.com/dswain07/CareerTrack---Azure-Functions.git
cd CareerTrack---Azure-Functions
```

### 2. Restore Dependencies and Build
```bash
dotnet restore CareerTrack.Api.ServiceLayer.slnx
dotnet build CareerTrack.Api.ServiceLayer.slnx
```

### 3. Configure Database
Ensure the `DefaultConnection` string in `CareerTrack.Api/appsettings.json` points to your SQLite database:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=careertrack.db"
  }
}
```
If initializing from scratch, apply Entity Framework Core migrations:
```bash
dotnet ef database update --project CareerTrack.Data --startup-project CareerTrack.Api
```

### 4. Configure Azure Storage Secrets
Never commit credentials to source control. Use ASP.NET Core User Secrets for local development:
```bash
cd CareerTrack.Api
dotnet user-secrets init
dotnet user-secrets set "AzureStorage:ConnectionString" "<YOUR_AZURE_STORAGE_CONNECTION_STRING>"
dotnet user-secrets set "AzureStorage:ContainerName" "careertrack-reports"
dotnet user-secrets set "AzureFunctions:BaseUrl" "https://careertrack-mp6-flex.azurewebsites.net/api"
```

### 5. Run the Application
From the `CareerTrack.Api` directory:
```bash
dotnet run
```
*Alternatively, set `CareerTrack.Api` as the startup project in Visual Studio and press **F5**.*

The API will bind to your local development ports (e.g., `http://localhost:5043` / `https://localhost:7220`).

### 6. Access the Web Interface
Open your browser and navigate to:
```text
http://localhost:5043
```
From the **Job Applications** tab, test the cloud integrations:
- **Calculate Score**
- **Follow-Up**
- **Generate Report**

---

## 9. Testing & Verification

| Test Scenario | Result |
| :--- | :--- |
| Azure Functions project builds cleanly | **Passed** |
| Application scoring executes locally | **Passed** |
| Follow-up recommendation executes locally | **Passed** |
| Both functions published to Azure | **Passed** |
| Application scoring executes in Azure | **Passed** |
| Follow-up recommendation executes in Azure | **Passed** |
| API-to-business-to-Azure communication pipeline | **Passed** |
| Web interface score calculation button | **Passed** |
| Web interface follow-up button | **Passed** |
| Azure Blob Storage report upload | **Passed** |
| Web interface Generate Report button | **Passed** |
| Existing application regression suite | **Passed** |

---

## 10. Security & Configuration

- **Secret Isolation:** Storage connection strings and Function URLs are managed via ASP.NET Core User Secrets in development and App Service Configuration in Azure.
- **Repository Safety:** `local.settings.json`, publish profiles, bin/obj folders, and database files are excluded via `.gitignore`.
- **Private Access:** The Azure Blob Storage container uses private access (anonymous read disabled).
- **Service Decoupling:** The client interface does not directly expose cloud infrastructure keys; requests are gated and authenticated through the backend API layer.

---

## 11. Future Improvements

- Implement user authentication and role-based access control (RBAC) with Microsoft Entra ID.
- Migrate Azure Functions from anonymous authorization to Function App Keys.
- Add document upload/download workflows for PDF resumes and cover letters in Azure Blob Storage.
- Automate follow-up notification emails through Azure Communication Services or SendGrid.
- Establish automated CI/CD deployment pipelines using GitHub Actions.

---

## 12. Graduate-Level Requirement

This project fulfills the graduate-level requirement through the complete integration of **Azure Blob Storage**:
- Provisioned and configured a dedicated Azure Storage Account.
- Configured a private `careertrack-reports` Blob container.
- Implemented `BlobReportService` using the official `Azure.Storage.Blobs` SDK.
- Created the `/api/jobapplications/{id}/upload-report` API endpoint.
- Integrated the user-facing **Generate Report** button in the web interface.
- Verified serialized JSON report artifacts directly in the Azure Portal.

## Author

Divyashanu Swain
