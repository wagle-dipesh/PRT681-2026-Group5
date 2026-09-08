# Tasks App — Setup, Architecture & Run Instructions

## Architecture
- **Backend:** ASP.NET Core Minimal API (`TasksApi`, Week 2) using Entity Framework Core over SQLite for persistence. Exposes REST endpoints under `/api/tasks` (GET/POST/PUT/DELETE), with input validation returning 400 and missing-resource lookups returning 404 (see Week 2 notes for the reasoning).
- **Frontend:** React app (`tasks-frontend`, Week 2, built with Vite) calling the API via `fetch`, with CORS enabled on the API for the frontend's origin, and explicit loading/error states rather than silent failure.
- **Containerisation:** The API is packaged with the `Dockerfile` in this folder (multi-stage build: SDK image builds/publishes, lightweight ASP.NET runtime image runs it — the final image doesn't ship the full SDK, only the compiled output and runtime, keeping it smaller).

```
tasks-frontend (React, port 5173) --HTTP (fetch)--> TasksApi (ASP.NET Core, port 8080 in container) --EF Core--> SQLite (tasks.db)
```

**Why SQLite instead of SQL Server here:** Week 1 deliberately used SQLite for zero-setup local persistence, and Week 2/3 kept that choice consistent rather than introducing a new database mid-exercise. The data-access code is isolated enough (all in `Program.cs`'s `TasksDbContext`) that swapping providers is a small change, not a rewrite — see the "Production considerations" section below for what that swap would actually involve.

## Run locally (without Docker)
```bash
# Backend
cd developer/Tianhui-Ke/Week_2/TasksApi
dotnet restore
dotnet run   # starts on http://localhost:5000 by default (varies by machine — check the terminal output for the actual port and update the frontend's API_URL to match)

# Frontend (separate terminal)
cd developer/Tianhui-Ke/Week_2/tasks-frontend
npm install
npm run dev  # starts on http://localhost:5173
```

## Run the API in Docker
```bash
cd developer/Tianhui-Ke/Week_3
docker build -t tasks-api -f Dockerfile ..
docker run -p 8080:8080 tasks-api
```
Verify it's up: `curl http://localhost:8080/api/tasks` should return `[]` on a fresh container (empty task list, not an error — see Week 2 notes on why an empty list and a broken API are deliberately distinguishable states).

## Deploying to a free tier (Azure App Service / Render)
**Azure App Service (Web App for Containers):**
1. Push the built image to a registry (Azure Container Registry or Docker Hub).
2. Create an App Service (Linux, Container) in a free/basic App Service Plan, pointing at that image.
3. Set `ASPNETCORE_URLS=http://+:8080` and any connection string as App Settings (environment variables) in the portal — never bake secrets into the Docker image itself.

**Render (alternative free-tier host):**
1. Create a new Web Service on Render, connect the GitHub repo, and point it at this `Dockerfile`.
2. Render builds the image automatically on push and exposes it on a public URL.

**Neither has actually been deployed for this exercise** — the practice task says "deploy to a free tier", but I'm documenting the exact steps rather than claiming a live URL I don't have, since I don't want to write down a deployment I haven't verified end-to-end. If required for the group demo, the next step is to actually run through one of these two paths and record the resulting URL here.

## Production considerations (what's simplified here, and what changes)
- **Database:** SQLite is fine for a single-instance demo; a real deployment on Azure App Service (which can restart/move the container) would lose the SQLite file on restart unless it's on persistent storage. Swapping to Azure SQL means: change the `Microsoft.EntityFrameworkCore.Sqlite` package to `Microsoft.EntityFrameworkCore.SqlServer`, change `UseSqlite(...)` to `UseSqlServer(...)`, and move the connection string to an App Setting instead of the hardcoded `"Data Source=tasks.db"`.
- **CORS:** currently hardcoded to `http://localhost:5173` in `Program.cs` — must be updated to the actual deployed frontend URL before this works outside local dev, or every frontend request will be silently blocked by the browser.
- **No authentication** (also flagged in Week 2 notes) — a real deployment needs at least an API key or JWT before it's reachable from the public internet.

## Notes for maintainers
- To run this locally you need the .NET 8 SDK and Node.js installed — versions aren't pinned here since this is a practice exercise, but a real project would pin exact versions in a `global.json` / `.nvmrc`.
