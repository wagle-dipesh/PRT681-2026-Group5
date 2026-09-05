# Task Manager

This is a small task manager made with React and ASP.NET Core. Tasks are stored in a local SQLite database.

## What you need

- .NET 10 SDK
- Node.js and npm
- Visual Studio 2022

## How to run it

Start the API first:

1. Open `TaskManagerAPI/TaskManagerAPI.slnx` in Visual Studio.
2. Choose **IIS Express** and run the project.

The API will run at `https://localhost:44366`. You can view the API documentation at `https://localhost:44366/swagger`.

Then start the React app in a new terminal:

```powershell
cd frontend
npm install
npm start
```

Open `http://localhost:3000` in your browser.

The database is created automatically in `TaskManagerAPI/tasks.db`.

## Running the API without Visual Studio

From the Week 6 folder, run:

```powershell
cd TaskManagerAPI
dotnet restore
dotnet run --launch-profile http
```

This runs the API at `http://localhost:5235`. To connect the frontend to it, change the API URL in `frontend/src/App.jsx` to:

```javascript
const API_URL = 'http://localhost:5235/api/tasks';
```