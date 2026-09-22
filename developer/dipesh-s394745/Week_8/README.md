# Employee Management Portal (Week 4 Practice)

A small "management portal" made of two parts:

1. **`backend/EmployeeApi`** — an ASP.NET Core Web API. It keeps a list of employees in memory and lets you Create, Read, Update, and Delete (CRUD) them.
2. **`frontend`** — a Next.js website that shows the employees in a Kendo UI React grid, and lets you add/edit/delete them through a form. It talks to the API using `fetch`.

This satisfies the Week 4 practice requirements: a Next.js app, a commercial component library (Kendo UI), CRUD screens, and an ASP.NET Core API — built with AI-assisted ("vibe coding") help from Claude Code.

## How to run it (2 terminals)

You need [Node.js](https://nodejs.org) and the [.NET SDK](https://dotnet.microsoft.com/download) installed.

**Terminal 1 — start the API:**

```bash
cd backend/EmployeeApi
dotnet run
```

Leave this running. It starts the API at `http://localhost:5184`.

**Terminal 2 — start the website:**

```bash
cd frontend
npm install
npm run dev
```

Leave this running too. Then open **http://localhost:3000** in your browser.

That's it — you should see a table of employees. Click **+ Add Employee** to add one, **Edit** to change one, or **Delete** to remove one. Every action calls the API in Terminal 1, so watch that terminal to see the requests come in.

## How it's built (for explaining to your professor)

### Backend (`backend/EmployeeApi`)

- [`Models/Employee.cs`](backend/EmployeeApi/Models/Employee.cs) — a plain C# class describing one employee (name, email, department, salary, etc).
- [`Program.cs`](backend/EmployeeApi/Program.cs) — the whole API. There's no real database - just a `List<Employee>` kept in memory, and five "minimal API" endpoints:

  | Method | Route                  | What it does        |
  |--------|------------------------|----------------------|
  | GET    | `/api/employees`       | list everyone        |
  | GET    | `/api/employees/{id}`  | get one employee     |
  | POST   | `/api/employees`       | add a new employee   |
  | PUT    | `/api/employees/{id}`  | update an employee   |
  | DELETE | `/api/employees/{id}`  | remove an employee   |

  A small `Validate()` function checks the data (name/email/etc. aren't blank) before saving, and returns a 400 error with a list of problems if something's wrong.

- `CORS` is turned on so the Next.js app (a different port) is allowed to call the API from the browser.

### Frontend (`frontend`)

- [`app/page.tsx`](frontend/app/page.tsx) — the Next.js **App Router** page. This is what makes it "Next.js" instead of plain React.
- [`components/EmployeePortal.tsx`](frontend/components/EmployeePortal.tsx) — the main screen. It fetches the employee list on load, shows it in a Kendo UI **Grid** (sortable, paged, searchable), and opens the Add/Edit form or a delete confirmation when you click a button.
- [`components/EmployeeForm.tsx`](frontend/components/EmployeeForm.tsx) — the Add/Edit form. Each field is just a `useState`, and a simple `validate()` function checks the form before it's submitted (so you can't send bad data to the API).
- [`lib/api.ts`](frontend/lib/api.ts) — the functions that call the API (`getEmployees`, `createEmployee`, etc.) using `fetch`.

### Things to point out in a demo

1. **Next.js App Router** — the `app/` folder structure, `page.tsx`.
2. **Kendo UI component library** — the Grid and form inputs are all from `@progress/kendo-react-*` (a commercial/enterprise component suite), not plain HTML.
3. **CRUD against a real API** — open your browser's dev tools (Network tab) while clicking Add/Edit/Delete, and you'll see the actual `GET`/`POST`/`PUT`/`DELETE` requests going to `http://localhost:5184`.
4. **Client + server validation** — try leaving "Email" blank in the form: the site blocks it before even calling the API. The API also checks again on its own, in case the request didn't come from the website.
5. **AI-assisted development ("vibe coding")** — this project was scaffolded and refined using Claude Code from a plain-English description of the assignment, rather than typing every file by hand.

## Notes

- Employee data resets every time you restart the API (`dotnet run`) - there's no real database, just a list in memory. That's fine for a class demo.
- The grid shows a "Kendo UI free trial" banner since no license key is set — normal for coursework, and fine to leave as-is (a paid key would be needed for a real company product).
