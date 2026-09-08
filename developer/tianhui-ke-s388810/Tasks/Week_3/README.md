# Theater Admin — Web API + jQuery Client

Week 3 task: re-create the same Movie/Category management functionality from the Week 2 MVC
app (`developer/tianhui-ke-s388810/Tasks/Week_2`), but architected as a **Web API backend +
jQuery client** instead of server-rendered MVC views.

## What changed from Week 2

| | Week 2 (MVC) | Week 3 (this project) |
|---|---|---|
| Controllers | Return `View()` with HTML | Return JSON (`[ApiController]`, `ActionResult<T>`) |
| Category dropdown | Built server-side with `SelectList` | Fetched via `GET /api/categories` and built with jQuery |
| Create/Edit forms | Separate Razor pages per action | One HTML page, one JS form, submits via `$.ajax` (POST for create, PUT for edit) |
| Page navigation | Full page reload per action (`asp-action` links) | Single page — JS shows/hides sections and re-fetches table data after each change, no reload |
| Validation | Data annotations + server-side `ModelState`, rendered with `asp-validation-for` | Same data annotations + server-side `ModelState` (still the source of truth) — errors are returned as JSON and displayed in JS |

The **Models and database schema are unchanged** (same `Movie`/`Category` classes, same
validation attributes, same Code First approach, same SQLite stand-in for SQL Express — see the
Week 2 README for that discussion, which still applies here).

## API endpoints

| Method | Route | Purpose |
|---|---|---|
| GET | `/api/movies` | List all movies (includes `categoryName` flattened in) |
| GET | `/api/movies/{id}` | Get one movie |
| POST | `/api/movies` | Create a movie |
| PUT | `/api/movies/{id}` | Update a movie |
| DELETE | `/api/movies/{id}` | Delete a movie |
| GET | `/api/categories` | List all categories |
| GET | `/api/categories/{id}` | Get one category |
| POST | `/api/categories` | Create a category |
| PUT | `/api/categories/{id}` | Update a category |
| DELETE | `/api/categories/{id}` | Delete a category (blocked with 400 if movies still reference it) |

## Why no separate frontend project / no CORS setup

The jQuery client (`wwwroot/index.html` + `wwwroot/js/app.js`) is served as static files by the
**same** ASP.NET Core app that hosts the API (`app.UseStaticFiles()` in `Program.cs`). This means
the browser calls `/api/movies` on the same origin the page was loaded from, so no CORS
configuration is needed. A genuinely separate frontend (its own dev server, different port) would
need `AddCors()` configured in `Program.cs` — noted here in case a future task asks for that
split explicitly.

## Setup & running locally

```bash
dotnet restore
dotnet ef migrations add InitialCreate
dotnet run --urls "http://localhost:5060"
```

Open `http://localhost:5060` in the browser — the jQuery client loads immediately and fetches
categories/movies from the API on page load.

(Port 5060 is just an example — pick whichever free port suits your machine; the client calls the
API via a relative path `/api/...`, so it always matches whatever port the app is actually
running on.)
