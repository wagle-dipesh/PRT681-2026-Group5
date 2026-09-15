# Week 6 — Docker & Deployment

## Architecture

The API runs in its own container; SQL Server runs in a separate container (the same one from
Week 4/5). They talk through the host machine's networking via Docker's `host.docker.internal`.

## Build the image
From the `Week_6` folder:
```bash
docker build -t tasks-api-week6 -f Dockerfile ..
```

## Run the container
```bash
docker run -p 8080:8080 \
  -e "ConnectionStrings__DefaultConnection=Server=host.docker.internal,1433;Database=WeeklyTasksDB;User Id=sa;Password=Passw0rd123;TrustServerCertificate=True;" \
  --name tasks-api-container \
  tasks-api-week6
```

Verify: `curl http://localhost:8080/api/tasks` should return the same task list as the
non-containerised version.

## Deploying to a free tier (Azure App Service or Render)
Not actually deployed live for this exercise — documenting the steps rather than claiming a URL
that doesn't exist:

1. Push the built image to a registry (Azure Container Registry or Docker Hub).
2. Create an App Service (Linux, Container) on a free/basic plan pointing at that image.
3. Set `ASPNETCORE_URLS` and `ConnectionStrings__DefaultConnection` as App Settings.
4. Real blocker to actually going live: `host.docker.internal` only resolves on this local
   machine — a cloud App Service would need a cloud-hosted database (e.g. Azure SQL Database).
