# TaskManagerMVC

A simple task-tracking web app built with ASP.NET Core MVC. It's a single server-rendered
application — Razor views act as the frontend, and the MVC controllers act as the API/backend
— backed by PostgreSQL via Dapper.

## Architecture

```
Browser (Razor views, Bootstrap)
        |
        v
TasksController / HomeController   <-- MVC controllers (request handling, validation)
        |
        v
ITaskRepository -> TaskRepository  <-- Dapper, plain parameterized SQL
        |
        v
   PostgreSQL (TaskManagerMvcDb)   <-- Tasks table
```

- **Views/** – Razor pages for the Home and Tasks screens (list, create, edit, details, delete).
- **Controllers/** – `HomeController` (landing/error pages) and `TasksController` (task CRUD).
- **Repositories/** – `ITaskRepository`/`TaskRepository`, the only layer that talks to the database.
- **Data/** – `IDbConnectionFactory`/`DbConnectionFactory`, builds `NpgsqlConnection`s from the
  `ConnectionStrings:DefaultConnection` configuration value.
- **Database/Setup.sql** – creates the `Tasks` table.
- **Models/** – `TaskItem` plus small view models for each page.

There is no separate SPA/API split: the controllers return HTML views directly, so
"containerizing the API" and "containerizing the frontend" are the same container here.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (for running outside Docker)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for the containerized setup)
- PostgreSQL, or the Dockerized PostgreSQL described below

## Run locally without Docker

1. Make sure a PostgreSQL instance is reachable and run `TaskManagerMVC/Database/Setup.sql`
   against it once (the database itself must already exist), to create the `Tasks` table.
2. Update the connection string in `TaskManagerMVC/appsettings.Development.json` /
   `appsettings.json` if your server isn't the default local instance.
3. From `TaskManagerMVC/TaskManagerMVC/`:
   ```
   dotnet run
   ```
4. Browse to the URL printed in the console (e.g. `https://localhost:7170`).

## Run locally with Docker

`docker-compose.yml` (in `TaskManagerMVC/TaskManagerMVC/`) spins up two services:

- `db` – PostgreSQL 16 in a container, with a named volume so data survives restarts. On first
  init it automatically runs `Database/Setup.sql` (mounted into `/docker-entrypoint-initdb.d`).
- `web` – builds the app from the `Dockerfile` and connects to `db`.

From `TaskManagerMVC/TaskManagerMVC/`:

```
docker compose up --build
```

Then browse to **http://localhost:8080**. A `/health` endpoint is also exposed for quick checks
(`curl http://localhost:8080/health`) and is what the Render deployment below uses as its
health-check path.

To reset the database, stop the stack and drop the volume:

```
docker compose down -v
```

### Configuration

The connection string is read from the standard ASP.NET Core configuration key
`ConnectionStrings:DefaultConnection`, which can be supplied as the environment variable
`ConnectionStrings__DefaultConnection` (double underscore). This is the same mechanism used
locally by `docker-compose.yml` and by the Render deployment below — nothing in the app code
needs to change between environments, only that one setting.

## Deployment

Both the app and the database are hosted on **Render**, entirely on its free tier:

1. **Create the database first.** In the [Render dashboard](https://dashboard.render.com/),
   **New > PostgreSQL**, free instance type. Once it's up, open its connection details and run
   `Database/Setup.sql` against it (e.g. via `psql` using the "External Connection String" Render
   gives you, or Render's built-in web shell) to create the `Tasks` table.
2. **Deploy the app.** Push this repo to GitHub (already the case here), then in the Render
   dashboard **New > Web Service**, connect this repository, and set:
   - **Root Directory**: `developer/dipesh-s394745/Week_7/TaskManagerMVC/TaskManagerMVC`
   - **Environment**: `Docker` (Render finds the `Dockerfile` in the root directory above)
   - **Instance Type**: `Free`
   - **Health Check Path**: `/health`
3. Add an environment variable on the web service:
   - `ConnectionStrings__DefaultConnection` = the database's **Internal Connection String** from
     step 1 (same-region services reach Render Postgres over the private network for free), in
     Npgsql format, e.g. `Host=<host>;Port=5432;Database=<db>;Username=<user>;Password=<password>;`
4. Deploy. Render assigns a `https://<service>.onrender.com` URL.

> Free-tier notes: Render's free web services spin down after periods of inactivity and take a
> few seconds to wake up on the next request. Free Postgres databases expire 30 days after
> creation (14-day grace period to upgrade before the data is deleted) — fine for a course demo,
> but not for anything you need to keep long-term without upgrading to a paid instance.

## Notes

- `Program.cs` adds forwarded-headers handling (`UseForwardedHeaders`) because Render terminates
  TLS at its edge and forwards plain HTTP to the container — without it, ASP.NET Core wouldn't
  know a request actually arrived over HTTPS.
- The Data Protection warning in the container logs ("keys may not be persisted...") is expected
  for a single-instance demo deployment like this one; it would matter for scaling to multiple
  instances or for anti-forgery tokens surviving a container restart, which is out of scope here.
