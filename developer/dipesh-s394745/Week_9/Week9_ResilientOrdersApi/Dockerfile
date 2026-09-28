# ---- Build stage ---------------------------------------------------------
# Use the SDK image (has the compiler) only to build and publish the app.
FROM mcr.microsoft.com/dotnet/sdk:8.0-alpine AS build
WORKDIR /src
COPY Week9_ResilientOrdersApi.csproj .
RUN dotnet restore
COPY . .
RUN dotnet publish -c Release -o /app/publish --no-restore

# ---- Final stage -----------------------------------------------------------
# The runtime-only image is much smaller than the SDK image, and Alpine is a
# lightweight Linux distribution - both keep the final container small.
FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS final
WORKDIR /app

# curl is needed for the HEALTHCHECK below - Alpine images don't include it by default.
RUN apk add --no-cache curl

COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080

# Docker (and docker-compose) will mark the container unhealthy if /health
# stops responding - this is what "test health checks locally" means in practice.
HEALTHCHECK --interval=15s --timeout=3s --start-period=20s --retries=5 \
    CMD curl -f http://localhost:8080/health || exit 1

ENTRYPOINT ["dotnet", "Week9_ResilientOrdersApi.dll"]
