# syntax=docker/dockerfile:1

# ---- Build stage ----
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY TaskManagerMVC.csproj ./
RUN dotnet restore ./TaskManagerMVC.csproj

COPY . .
RUN dotnet publish ./TaskManagerMVC.csproj -c Release -o /app/publish --no-restore

# ---- Runtime stage ----
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_ENVIRONMENT=Production \
    DOTNET_RUNNING_IN_CONTAINER=true

COPY --from=build /app/publish .
USER app

EXPOSE 8080
ENTRYPOINT ["dotnet", "TaskManagerMVC.dll"]
