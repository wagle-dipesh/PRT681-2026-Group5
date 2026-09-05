# Week 6 Task Manager

Full-stack task manager built with a React frontend and an ASP.NET Core Web API backend using SQLite.

## Prerequisites

- .NET 10 SDK
- Node.js 18 or newer and npm
- Visual Studio 2022 (recommended for the IIS Express HTTPS profile)

Check the installed versions:

```powershell
dotnet --version
node --version
npm --version
```

## Run with Visual Studio

This is the recommended setup because the frontend is configured to call the IIS Express HTTPS API at `https://localhost:44366/api/tasks`.

1. Open `TaskManagerAPI/TaskManagerAPI.slnx` in Visual Studio.
2. Select **IIS Express** as the startup profile.
3. Start the API. The Swagger page should open at `https://localhost:44366/swagger`.
4. In a separate terminal, install and start the frontend:

```powershell
cd frontend
npm install
npm start
```

5. Open `http://localhost:3000` in a browser.

The SQLite database is created automatically as `TaskManagerAPI/tasks.db` when the API starts.

## Run the API from the terminal

From the Week 6 directory:

```powershell
cd TaskManagerAPI
dotnet restore
dotnet run --launch-profile http
```

This starts the API at `http://localhost:5235`. Swagger is available at `http://localhost:5235/swagger`.

The current frontend uses the IIS Express URL. To use the terminal profile instead, update `API_URL` in `frontend/src/App.jsx` to:

```javascript
const API_URL = 'http://localhost:5235/api/tasks';
```

Then restart the React development server.

## Useful commands

Frontend commands are run from `frontend`:

```powershell
npm start       # Start the development server
npm test        # Run React tests
npm run build   # Create a production build
```

Backend commands are run from `TaskManagerAPI`:

```powershell
dotnet build    # Build the API
dotnet run      # Run using the default profile
```

## Troubleshooting

- If the browser reports a certificate warning for `localhost`, trust the local ASP.NET Core development certificate with `dotnet dev-certs https --trust`.
- If the task list cannot load, confirm that the API is running at the URL configured in `frontend/src/App.jsx`.
- If the browser reports a CORS error, use `http://localhost:3000` for the React app. That origin is allowed by the API configuration.