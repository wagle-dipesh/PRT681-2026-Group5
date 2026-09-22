# Week 4 — Setup Instructions

## Prerequisites
- Node.js installed (already used for the React frontend in earlier weeks).
- The Tasks API running (either local `dotnet run` on port 5000, or the Week 6 Docker container
  on port 8080 — update `NEXT_PUBLIC_API_URL` below to match whichever is running).
- A free KendoReact trial license (see below) — the grid renders without one but shows a trial
  banner/watermark.

## Get a free KendoReact trial license
1. Go to https://www.telerik.com/kendo-react-ui/ and sign up for a free trial account.
2. Follow their instructions to activate the trial license locally (usually `npx kendo-ui-license activate`
   after installing the packages, using the license key from your Telerik account).
3. Without this step, the grid still works for local practice/demo purposes — it just shows a
   trial watermark, which is expected and fine for coursework.

## Run the portal

```bash
cd tasks-portal
npm install
npm run dev
```

Open `http://localhost:3000`.

If your API is running on a different port than 8080, set the environment variable before running:
```bash
NEXT_PUBLIC_API_URL=http://localhost:5000/api/tasks npm run dev
```

## What to look at
- `app/page.tsx` — the Server Component that fetches the initial task list server-side (SSR).
- `components/TasksGrid.tsx` — the Client Component (`"use client"`) that hydrates into an
  interactive, editable KendoReact Grid.
