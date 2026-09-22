# Week 8 – Dockerized Task Manager

## Overview

This project is a full-stack Task Manager application developed using React, KendoReact, ASP.NET Core Web API, Entity Framework Core and SQL Server.

For Week 8, the complete application was containerized using Docker and Docker Compose. The frontend, API and database run as separate containers and communicate with each other.

## Features

- Create a new task
- Display all tasks
- Edit an existing task
- Mark a task as completed or pending
- Delete a task
- Store task data in SQL Server
- Run the complete application using Docker Compose

## Technologies Used

### Frontend

- React
- Vite
- JavaScript
- KendoReact
- Kendo UI Default Theme
- Nginx

### Backend

- ASP.NET Core Web API
- C#
- Entity Framework Core
- REST API

### Database

- Microsoft SQL Server 2022

### Containerization

- Docker
- Docker Compose

## Application Architecture

The application consists of three Docker services:

1. **Frontend** – React and KendoReact application served through Nginx.
2. **API** – ASP.NET Core Web API that provides CRUD endpoints.
3. **Database** – SQL Server database used to store task information.

Application flow:

```text
User → React/KendoReact Frontend → ASP.NET Core Web API → SQL Server
```

Docker Compose creates and connects all three services.

## Project Structure

```text
Week_8/
├── TaskManagerApi/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Dockerfile
│   └── Program.cs
├── task-manager-client/
│   ├── src/
│   ├── Dockerfile
│   └── package.json
├── docker-compose.yml
├── .env.example
└── README.md
```

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| GET | `/api/tasks` | Retrieve all tasks |
| GET | `/api/tasks/{id}` | Retrieve one task |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks/{id}` | Update an existing task |
| DELETE | `/api/tasks/{id}` | Delete a task |

## Running the Application with Docker

### Prerequisites

Install the following software:

- Docker Desktop
- Git

Make sure Docker Desktop is running before starting the application.

### 1. Clone the repository

```bash
git clone <your-github-repository-url>
cd PRT681-2026-Group5/developer/krijal-s385427/Week_8
```

### 2. Configure the environment file

Create a file named `.env` inside the `Week_8` folder.

Add the following variable:

```env
MSSQL_SA_PASSWORD=YourStrongPasswordHere
```

The password must meet SQL Server password complexity requirements. Do not commit the `.env` file to GitHub.

An `.env.example` file is included to show the required configuration.

### 3. Build and run the containers

From the `Week_8` folder, run:

```bash
docker compose up --build
```

Docker Compose will start:

- SQL Server
- ASP.NET Core Web API
- React frontend

### 4. Open the application

Frontend:

```text
http://localhost:3000
```

API:

```text
http://localhost:8080/api/tasks
```

An empty API response such as `[]` means the API is working but no tasks have been created yet.

## Stopping the Application

Press `Ctrl + C` in the terminal and run:

```bash
docker compose down
```

To start it again:

```bash
docker compose up
```

## Database Persistence

The SQL Server data is stored in a Docker volume named:

```text
taskmanager-sql-data
```

This allows task data to remain available after the containers are stopped and restarted.

Running `docker compose down` does not normally delete the database volume.

## Testing Completed

The following operations were tested successfully:

- Docker images built successfully
- All three containers started successfully
- Frontend connected to the API
- API connected to SQL Server
- Created a task
- Retrieved tasks
- Updated a task
- Changed task completion status
- Deleted a task
- Confirmed database persistence

## Deployment Status

Local Docker deployment is complete.

The remaining task is to deploy the complete application to Azure App Service or Render and add the deployed application URL to this README.

## Author

Krijal Khadka  
Master of Information Technology  
Charles Darwin University