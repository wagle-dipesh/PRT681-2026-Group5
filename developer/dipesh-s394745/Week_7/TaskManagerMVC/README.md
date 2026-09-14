# TaskManagerMVC

A task management web application built with ASP.NET Core MVC. It uses Razor views for the
frontend and MVC controllers for the backend, with PostgreSQL as the database, accessed through
Dapper.

**Live demo:** https://prt681-2026-group5.onrender.com/Tasks

(hosted on Render's free tier — the app spins down after inactivity, so the first request may take
a few seconds to respond)

## Architecture

```
Browser (Razor views, Bootstrap)
        |
        v
Controllers (HomeController, TasksController)
        |
        v
ITaskRepository / TaskRepository (Dapper)
        |
        v
PostgreSQL (TaskManagerMvcDb)
```

- **Views/** – Razor pages for the Home and Tasks screens (list, create, edit, details, delete).
- **Controllers/** – `HomeController` for the landing/error pages and `TasksController` for task CRUD.
- **Repositories/** – `ITaskRepository` / `TaskRepository`, the only layer that talks to the database.
- **Data/** – `IDbConnectionFactory` / `DbConnectionFactory`, creates database connections from the
  connection string in configuration.
- **Database/Setup.sql** – creates the `Tasks` table.
- **Models/** – `TaskItem` and the view models used by each page.

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- PostgreSQL
- Dapper
- Docker

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for the containerized setup)
- PostgreSQL (local install, or use the Dockerized setup below)

## Running Locally (without Docker)

1. Make sure a PostgreSQL instance is running and the database exists, then run
   `TaskManagerMVC/Database/Setup.sql` against it once to create the `Tasks` table.
2. Update the connection string in `TaskManagerMVC/appsettings.Development.json` or
   `appsettings.json` to match your PostgreSQL server.
3. From `TaskManagerMVC/TaskManagerMVC/`, run:
   ```
   dotnet run
   ```
4. Open the URL shown in the console (e.g. `https://localhost:7170`).

## Running Locally (with Docker)

`docker-compose.yml` (in `TaskManagerMVC/TaskManagerMVC/`) starts two services:

- **db** – PostgreSQL 16, with a named volume so data persists between restarts. It runs
  `Database/Setup.sql` automatically on first startup.
- **web** – the app, built from the `Dockerfile`.

From `TaskManagerMVC/TaskManagerMVC/`:

```
docker compose up --build
```

Then open **http://localhost:8080**. The app also exposes a `/health` endpoint used for health
checks in deployment.

To reset the database:

```
docker compose down -v
```

### Configuration

The connection string is read from `ConnectionStrings:DefaultConnection`, which can also be set
using the environment variable `ConnectionStrings__DefaultConnection`. This is the same setting
used by Docker Compose and by the Render deployment, so nothing in the code changes between
environments.

## Deployment

The app and database are both hosted on Render (free tier).

1. **Create the database.** In the Render dashboard, choose **New > PostgreSQL** and select the
   free instance type. Once it's running, run `Database/Setup.sql` against it (for example with
   `psql`, using the External Database URL from the Render dashboard) to create the `Tasks` table.
2. **Deploy the app.** Push the repository to GitHub, then in the Render dashboard choose
   **New > Web Service** and connect the repository. Configure:
   - Root Directory: `developer/dipesh-s394745/Week_7/TaskManagerMVC/TaskManagerMVC`
   - Environment: `Docker`
   - Instance Type: `Free`
   - Health Check Path: `/health`
   - Region: same region as the database
3. Add an environment variable to the web service:
   - `ConnectionStrings__DefaultConnection`, using the database's connection details in Npgsql
     format:
     ```
     Host=<host>;Port=5432;Database=<db>;Username=<user>;Password=<password>;
     ```
     Render shows connection details as a `postgresql://` URL by default — this needs to be
     converted to the format above, since Npgsql doesn't accept the URL format directly.
4. Deploy. Render will build and host the app at `https://<service-name>.onrender.com`.

Notes on the free tier: web services spin down after periods of inactivity and take a few seconds
to restart on the next request. Free PostgreSQL databases expire 30 days after creation (with a
14-day grace period before deletion), which is fine for a course project but not for long-term use
without upgrading.

## Notes

- `Program.cs` uses `UseForwardedHeaders` because Render terminates HTTPS at its edge and forwards
  plain HTTP to the container.
- The Data Protection warning seen in the logs ("keys may not be persisted...") is expected for a
  single-instance deployment and doesn't affect functionality here.
