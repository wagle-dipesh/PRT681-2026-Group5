# Task Manager Web Application

## Overview

This project converts the previous Task Manager Console Application into a full web application.

It uses an ASP.NET Core Web API for the backend, Entity Framework Core for database access and React with KendoReact for the frontend.

The application was developed as the Week 7 developer practice task for Web API and frontend basics.

## Application Architecture

```text
KendoReact Frontend
        ↓ HTTP Requests and JSON
ASP.NET Core Web API
        ↓
Entity Framework Core
        ↓
SQL Server Express
```

## Project Structure

```text
Week_7/
├── TaskManagerApi/
│   ├── Controllers/
│   ├── Data/
│   ├── Models/
│   ├── Program.cs
│   └── appsettings.json
│
├── task-manager-client/
│   ├── src/
│   ├── package.json
│   └── vite.config.js
│
└── README.md
```

## Technologies

### Backend

- C#
- .NET
- ASP.NET Core Web API
- Entity Framework Core
- SQL Server Express
- OpenAPI

### Frontend

- React.js
- JavaScript
- Vite
- KendoReact
- HTML
- CSS
- Fetch API

## Features

The application allows users to:

- View all tasks
- Add a new task
- Edit an existing task
- Mark a task as completed or pending
- Delete a task
- Validate task information
- Save task data in SQL Server
- Access task data through REST API endpoints

## Database

The application uses the existing SQL Server database:

```text
TaskManagerDb
```

The `Tasks` table contains:

- `Id`
- `Title`
- `Description`
- `IsCompleted`
- `CreatedAt`

The SQL Server instance is:

```text
.\SQLEXPRESS
```

Windows Authentication is used for the database connection.

## REST API Endpoints

| HTTP method | Endpoint | Description |
|---|---|---|
| GET | `/api/tasks` | Retrieve all tasks |
| GET | `/api/tasks/{id}` | Retrieve one task |
| POST | `/api/tasks` | Create a new task |
| PUT | `/api/tasks/{id}` | Update an existing task |
| DELETE | `/api/tasks/{id}` | Delete an existing task |

## Running the Backend

Open a terminal in the API folder:

```cmd
cd TaskManagerApi
```

Restore the dependencies:

```cmd
dotnet restore
```

Run the API:

```cmd
dotnet run
```

The API currently runs at:

```text
http://localhost:5150
```

Test the API using:

```text
http://localhost:5150/api/tasks
```

## Running the Frontend

Open another terminal in the React project:

```cmd
cd task-manager-client
```

Install the dependencies:

```cmd
npm install
```

Run the React application:

```cmd
npm run dev
```

Open the frontend using the address displayed by Vite, normally:

```text
http://localhost:5173
```

Both the API and React application must be running at the same time.

## Related Learning Courses

### ASP.NET Core: Building RESTful APIs

- Creating API controllers and routes
- Working with HTTP methods and status codes
- Sending and receiving JSON data

### Entity Framework Core Essential Training

- Creating entities and database contexts
- Connecting ASP.NET Core to SQL Server
- Querying and updating database records

### React.js Essential Training

- Creating React components
- Managing state with `useState`
- Loading API data with `useEffect`

### KendoReact Practice

- Using KendoReact input components
- Using KendoReact buttons
- Applying the KendoReact default theme

## Learning Outcomes

Through this task, I practised:

- Converting a console application into a web application
- Developing REST API endpoints
- Connecting an API to SQL Server
- Using Entity Framework Core
- Building a React frontend
- Using KendoReact UI components
- Calling an API using the Fetch API
- Configuring CORS
- Implementing complete CRUD operations
- Using Git and GitHub for version control

## Author

**Krijal Khadka**  
**Student ID:** s385427