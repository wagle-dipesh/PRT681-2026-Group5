# Task Manager

A small full stack task manager we made to get used to the classic CRUD operations: create, read, update, delete. Backend is ASP.The backend is written in NET Core, the front-end in React, and it's stored in a local SQLite file, meaning you don't need to install a database server.


## What it actually does

It's a to-do list. You can:
1. View all your tasks
2. Fill out the rest of the document in groups.
3. When finished, check it off.
4. When no longer needed, delete it

## How the pieces talk to each other

```
React (localhost:3000)  →  ASP.NET Core API (localhost:5235 / 44366)  →  SQLite (tasks.db)
```

The React app does not access the database directly, it merely calls the API. The API is the only service that accesses and modifies `tasks.db`, instead of using raw SQL it does this with Entity Framework Core. We enabled CORS on the API so that the browser (on a different port) can call into the API: otherwise, the browser would deny it by default.

## Backend — `TaskManagerAPI/`

We divide this into several folders rather than into a single controller file:

- **`Controllers/TasksController.cs`** — This is where you receive the HTTP requests (GET, POST, PUT, DELETE) and get back a response (back to the browser, etc.). It is not concerned with the way tasks are stored, it just requests tasks from the repository.
- **`Repositories/`** —  This is where the actual database operations occur (Add, update, delete, query). This folder is not connected to the controller so if we changed the database from SQLite to another format, we would only need to change this folder.
- **`Models/TaskItem.cs`** — This is the model of a task in the database including validation (must have title, must be no more than 100 characters etc.). This is where the data goes into and out of the API
- **`DTOs/TaskDto.cs`** — We don't expose the actual model of the database to the outside world, we will specify exactly what a "create task" request should look like, and exactly what we should return.
- **`Data/AppDbContext.cs`** — this is the EF Core bridge to the actual SQLite file.
- **`Program.cs`** — starts up the program (database connection, CORS, Swagger etc.)

### Endpoints

| Method | Route | What it does |
|---|---|---|
| GET | `/api/tasks` | get all tasks |
| GET | `/api/tasks/{id}` | get one task by id |
| POST | `/api/tasks` | create a new task |
| PUT | `/api/tasks/{id}` | updates task |
| DELETE | `/api/tasks/{id}` | deletes task |

The API's running, and if you're curious, you can also explore at `/swagger` which will provide you with a UI to try out all the endpoints, without having to get Postman!

You don't need to create the database file, (tasks.db), it is automatically generated the first time you run the API.

## Frontend — `frontend/` (source lives in `src/`)

- **`App.jsx`** — the “brain” of the application. It fetches the tasks whenever the page is loaded and stores them in the state. All operations of adding/deleting/toggling eventually pass through this component.
- **`TaskForm.jsx`** — It is just a form that allows users to create a new task. However, the function does not call the API. It simply gives the new task back to `App.jsx`, which calls the API.
- **`TaskItem.jsx`** — creates one item (checkbox, title, description, delete button). Again, clicking on anything in this component will invoke functions assigned to it by `App.jsx`.

Basically: Data is all in `App.jsx`. The other two components are only responsible for displaying stuff and informing `App.jsx` when something happens. This is pretty much a typical React pattern and also makes debugging easy.

## Running it locally

**Backend — pick one:**

Using Visual Studio: open `TaskManagerAPI/TaskManagerAPI.slnx`, hit run with IIS Express. It'll come up on `https://localhost:44366`.

Without Visual Studio:
```powershell
cd TaskManagerAPI
dotnet restore
dotnet run --launch-profile http
```
This runs it on `http://localhost:5235` instead — if you go this route, update the API URL in `frontend/src/App.jsx`:
```javascript
const API_URL = 'http://localhost:5235/api/tasks';
```

**Frontend:**
```powershell
cd frontend
npm install
npm start
```
Then just open `http://localhost:3000`.

## A couple of facts that you should know before handling the code

- Adding a new field to `TaskItem` class, don't forget that this field will not be returned in the API response until it is added to DTO as well — simple mistake which can cost some time figuring out why new field doesn't exist.
- It's best to implement all new database logic in the repository, rather than in the controller — it is always tempting to put it right there, but that kind of defeats the purpose of separating logic into different classes.
- Frontend code assumes that all fields are camelCase (`isCompleted`, `createdAt`) — ASP.NET Core sends all data in such format by default, changing that format will break front end functionality.
