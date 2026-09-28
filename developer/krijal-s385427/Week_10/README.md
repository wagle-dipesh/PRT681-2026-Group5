# Week 10 – Resilient Task Manager API

This project extends the Task Manager application with structured logging, centralized monitoring, exception handling, health checks, asynchronous email delivery, durable workflows and Linux containerization.

## Technologies

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Serilog
- Seq
- MailKit
- Mailpit
- Temporal.io
- Docker and Docker Compose
- Linux-based .NET containers

## Features

- Task CRUD operations
- Structured application and HTTP request logging
- Centralized log monitoring through Seq
- Global exception handling with safe JSON responses
- Exception telemetry recorded in Seq
- ASP.NET Core health-check endpoint
- Docker container health checks
- SMTP email delivery using MailKit
- Local email testing through Mailpit
- Durable email workflow using Temporal
- Automatic retry of failed email activities
- SQL Server data persistence
- Multi-stage Linux Docker image

## Project Structure

```text
Week_10
├── docker-compose.yml
├── .env
├── README.md
└── TaskManagerApi
    ├── Controllers
    ├── Data
    ├── Middleware
    │   └── GlobalExceptionMiddleware.cs
    ├── Models
    │   └── EmailRequest.cs
    ├── Services
    │   ├── IEmailService.cs
    │   └── EmailService.cs
    ├── Settings
    │   └── EmailSettings.cs
    ├── Workflows
    │   ├── EmailActivities.cs
    │   └── EmailWorkflow.cs
    ├── Dockerfile
    ├── Program.cs
    └── appsettings.json
```

## Docker Services

| Service | Purpose | Address |
|---|---|---|
| Task Manager API | REST API | http://localhost:8081 |
| SQL Server | Application database | localhost:14330 |
| Seq | Structured log dashboard | http://localhost:5341 |
| Mailpit | Test email inbox | http://localhost:8025 |
| Temporal | Workflow orchestration | localhost:7233 |
| Temporal UI | Workflow dashboard | http://localhost:8233 |

## Environment Configuration

Create a `.env` file inside `Week_10`:

```env
MSSQL_SA_PASSWORD=YourStrongPassword123!
```

The `.env` file contains sensitive information and must not be committed to Git.

## Running the Application

Make sure Docker Desktop is running.

From the `Week_10` folder:

```cmd
docker compose up --build -d
```

Check the containers:

```cmd
docker compose ps
```

Stop the application:

```cmd
docker compose down
```

## API Endpoints

| Method | Endpoint | Description |
|---|---|---|
| GET | `/` | API status |
| GET | `/health` | Container health check |
| GET | `/api/tasks` | Retrieve tasks |
| POST | `/api/tasks` | Create a task |
| PUT | `/api/tasks/{id}` | Update a task |
| DELETE | `/api/tasks/{id}` | Delete a task |
| POST | `/api/email/send` | Send an email directly with MailKit |
| POST | `/api/email/workflow` | Dispatch email through Temporal |
| GET | `/test-error` | Test exception tracking in Development |

## Testing the Health Check

Open:

```text
http://localhost:8081/health
```

Expected response:

```text
Healthy
```

Docker should also show the API as healthy:

```cmd
docker compose ps
```

## Testing Direct Email Delivery

```cmd
curl -X POST http://localhost:8081/api/email/send -H "Content-Type: application/json" -d "{\"recipient\":\"student@example.com\",\"subject\":\"MailKit Test\",\"body\":\"Direct MailKit email delivery is working.\"}"
```

Open Mailpit at http://localhost:8025 to view the email.

## Testing the Temporal Email Workflow

```cmd
curl -X POST http://localhost:8081/api/email/workflow -H "Content-Type: application/json" -d "{\"recipient\":\"student@example.com\",\"subject\":\"Temporal Workflow Test\",\"body\":\"This email was dispatched through a durable Temporal workflow.\"}"
```

The API returns a workflow ID. The workflow can be inspected through the Temporal UI at http://localhost:8233.

The completed email appears in Mailpit at http://localhost:8025.

## Logging and Exception Testing

Open the following URL to generate a test exception:

```text
http://localhost:8081/test-error
```

The API returns a safe JSON error response containing a trace ID. The full exception is recorded in Seq.

Open Seq:

```text
http://localhost:5341
```

Seq displays:

- HTTP request information
- Response status codes
- Request duration
- Entity Framework database activity
- Email activity events
- Temporal activity events
- Unhandled exceptions

## Durable Workflow

The email workflow uses a Temporal activity to call the MailKit email service. The activity has a maximum of three attempts, allowing temporary email or network failures to be retried.

```text
API request
    → Temporal workflow
    → Email activity
    → MailKit SMTP service
    → Mailpit inbox
```

Temporal retains workflow state, allowing execution to continue after temporary failures or application restarts.

## Learning Outcomes

This task provided practical experience with:

- Structured and centralized logging
- Production-style exception handling
- Application and container health checks
- SMTP communication using MailKit
- Durable workflow orchestration
- Retry policies and failure recovery
- Docker networking and service configuration
- Linux container deployment
- Secure environment configuration