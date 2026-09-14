# TaskManagerMVC

A simple task-tracking web app built with ASP.NET Core MVC. It's a single server-rendered
application — Razor views act as the frontend, and the MVC controllers act as the API/backend
— backed by SQL Server via Dapper and a stored procedure.

## Architecture

```
Browser (Razor views, Bootstrap)
        |
        v
TasksController / HomeController   <-- MVC controllers (request handling, validation)
        |
        v
ITaskRepository -> TaskRepository  <-- Dapper, calls dbo.spTasks_Manager
        |
        v
   SQL Server (TaskManagerMvcDb)   <-- Tasks table + spTasks_Manager stored procedure
```

- **Views/** – Razor pages for the Home and Tasks screens (list, create, edit, details, delete).
- **Controllers/** – `HomeController` (landing/error pages) and `TasksController` (task CRUD).
- **Repositories/** – `ITaskRepository`/`TaskRepository`, the only layer that talks to the database.
- **Data/** – `IDbConnectionFactory`/`DbConnectionFactory`, builds `SqlConnection`s from the
  `ConnectionStrings:DefaultConnection` configuration value.
- **Database/Setup.sql** – creates the `TaskManagerMvcDb` database, the `Tasks` table, and the
  `spTasks_Manager` stored procedure (insert/select/select-all/update/delete via an `@Action` flag).
- **Models/** – `TaskItem` plus small view models for each page.

There is no separate SPA/API split: the controllers return HTML views directly, so
"containerizing the API" and "containerizing the frontend" are the same container here.

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download) (for running outside Docker)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for the containerized setup)
- SQL Server, LocalDB, or the Dockerized SQL Server described below

## Run locally without Docker

1. Make sure a SQL Server instance is reachable and run `TaskManagerMVC/Database/Setup.sql`
   against it once, to create the database, table, and stored procedure.
2. Update the connection string in `TaskManagerMVC/appsettings.Development.json` /
   `appsettings.json` if your server isn't the default local instance.
3. From `TaskManagerMVC/TaskManagerMVC/`:
   ```
   dotnet run
   ```
4. Browse to the URL printed in the console (e.g. `https://localhost:7170`).

## Run locally with Docker

`docker-compose.yml` (in `TaskManagerMVC/TaskManagerMVC/`) spins up three services:

- `db` – SQL Server 2022 in a container, with a named volume so data survives restarts.
- `db-init` – waits for `db` to accept connections, then runs `Database/Setup.sql` once.
- `web` – builds the app from the `Dockerfile` and connects to `db`.

From `TaskManagerMVC/TaskManagerMVC/`:

```
docker compose up --build
```

Then browse to **http://localhost:8080**. A `/health` endpoint is also exposed for quick checks
(`curl http://localhost:8080/health`) and is what the cloud deployments below use as their
health-check path.

To reset the database, stop the stack and drop the volume:

```
docker compose down -v
```

### Configuration

The connection string is read from the standard ASP.NET Core configuration key
`ConnectionStrings:DefaultConnection`, which can be supplied as the environment variable
`ConnectionStrings__DefaultConnection` (double underscore). This is the same mechanism used
locally by `docker-compose.yml`, and by both cloud deployments below — nothing in the app code
needs to change between environments, only that one setting.

## Deployment

Because this app depends on SQL Server specifically (T-SQL, a stored procedure, and
`Microsoft.Data.SqlClient`), the database side works best on **Azure SQL Database**, which has an
always-free tier (one free database per subscription, auto-pauses when idle). Two options for
hosting the app itself:

### Option A — Render (runs the actual Docker image, recommended)

Render's free Web Service plan builds and runs a `Dockerfile` directly, so this is the option
that deploys the exact container built above.

1. Push this repo to GitHub (already the case here).
2. In the [Render dashboard](https://dashboard.render.com/), **New > Web Service**, connect this
   repository.
3. Set:
   - **Root Directory**: `developer/dipesh-s394745/Week_7/TaskManagerMVC/TaskManagerMVC`
   - **Environment**: `Docker` (Render finds the `Dockerfile` in the root directory above)
   - **Instance Type**: `Free`
   - **Health Check Path**: `/health`
4. Add an environment variable:
   - `ConnectionStrings__DefaultConnection` = the Azure SQL connection string, e.g.
     `Server=tcp:<server>.database.windows.net,1433;Initial Catalog=TaskManagerMvcDb;User ID=<user>;Password=<password>;Encrypt=True;TrustServerCertificate=False;`
5. Deploy. Render assigns a `https://<service>.onrender.com` URL.

Provision the Azure SQL free database first (see `deploy/azure-deploy.ps1`, or create it manually
in the Azure Portal: SQL Database > "Free offer" edition) and run `Database/Setup.sql` against it
once, using its public endpoint.

> Free-tier note: Render's free web services spin down after periods of inactivity and take a
> few seconds to wake up on the next request.

### Option B — Azure App Service (Free F1 tier)

Azure's Free (F1) App Service tier does **not** support custom containers, so this path deploys
the published app code directly (Azure builds it with Oryx) rather than the Docker image — Docker
is still used for local development and Option A above.

Run the provisioning script from `deploy/azure-deploy.ps1` after `az login`:

```powershell
az login
./deploy/azure-deploy.ps1
```

It creates, in order: a resource group, an Azure SQL free-tier database (with the firewall rule
Azure services need), a Free (F1) App Service plan, and a Web App wired up with the connection
string and a `/health` health check, then deploys the app from source. See the script's comments
for what each step does; it prompts for a new SQL admin password and prints the app URL at the end.

> Free-tier note: F1 App Service apps are throttled (limited daily compute minutes) and have no
> custom domain/TLS support, but are otherwise fully functional for a small demo app like this.

## Notes

- `Program.cs` adds forwarded-headers handling (`UseForwardedHeaders`) because both Render and
  Azure App Service terminate TLS at their edge and forward plain HTTP to the container — without
  it, ASP.NET Core wouldn't know a request actually arrived over HTTPS.
- The Data Protection warning in the container logs ("keys may not be persisted...") is expected
  for a single-instance demo deployment like this one; it would matter for scaling to multiple
  instances or for anti-forgery tokens surviving a container restart, which is out of scope here.
