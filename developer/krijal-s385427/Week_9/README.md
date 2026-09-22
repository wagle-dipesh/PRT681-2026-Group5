# Week 9 – Next.js Enterprise Task Management Portal

## Overview

This project is a responsive task management portal developed using Next.js, TypeScript and KendoReact. It connects to the existing ASP.NET Core Web API and SQL Server database created during Week 8.

The project demonstrates enterprise UI components, server-side data retrieval, client-side hydration, client-side validation and AI-assisted development.

## Technologies Used

### Frontend

- Next.js App Router
- React
- TypeScript
- Tailwind CSS
- KendoReact Grid
- KendoReact Inputs
- KendoReact Buttons

### Backend

- ASP.NET Core Web API
- Entity Framework Core
- SQL Server
- Docker and Docker Compose

## Features

- Responsive management dashboard
- Server-side initial task retrieval
- Interactive KendoReact Grid
- Create, read, update and delete operations
- Client-side form validation
- Task completion status
- Grid sorting
- Grid filtering
- Grid pagination
- Loading and saving states
- Success and error messages
- Delete confirmation
- Dashboard task statistics

## Architecture

```text
User
  ↓
Next.js Management Portal
  ↓
ASP.NET Core Web API
  ↓
Entity Framework Core
  ↓
SQL Server
```

The Next.js portal runs separately from the ASP.NET Core API.

The initial task data is retrieved by the Next.js Server Component. The data is passed to the KendoReact client component, where React hydration enables filtering, sorting, pagination and CRUD interactions.

## Server-Side Rendering and Hydration

The main `page.tsx` file is a Server Component. It retrieves tasks from the ASP.NET Core API before rendering the page.

The `TaskGrid` and `TaskForm` files are Client Components. They use React state, event handlers and KendoReact components to provide browser-based interaction.

This demonstrates the difference between:

- Server-side data retrieval and rendering
- Client-side hydration and interactivity

## CRUD Operations

| Operation      | HTTP method | API endpoint      |
| -------------- | ----------- | ----------------- |
| Retrieve tasks | GET         | `/api/tasks`      |
| Create task    | POST        | `/api/tasks`      |
| Update task    | PUT         | `/api/tasks/{id}` |
| Delete task    | DELETE      | `/api/tasks/{id}` |

## Client-Side Validation

The task form validates information before sending it to the API:

- Task title is required
- Task title must contain at least three characters
- Task title cannot exceed 200 characters
- Description cannot exceed 1,000 characters
- Validation messages are displayed beside invalid fields
- The form clears only after a successful API request

The ASP.NET Core API remains responsible for final server-side validation and database operations.

## AI-Assisted Development

AI-assisted engineering was used throughout the project to:

- Plan the frontend architecture
- Scaffold TypeScript task interfaces
- Generate the reusable API service
- Scaffold the KendoReact Grid and task form
- Implement CRUD operations
- Add client-side validation
- Refactor repeated logic
- Resolve TypeScript compatibility issues
- Update the KendoReact custom-cell implementation
- Improve error handling and application feedback

All AI-generated suggestions were reviewed, tested and adjusted before being included in the project.

## Project Structure

```text
Week_9/
├── README.md
└── task-manager-portal/
    ├── src/
    │   ├── app/
    │   │   ├── layout.tsx
    │   │   └── page.tsx
    │   ├── components/
    │   │   ├── TaskForm.tsx
    │   │   └── TaskGrid.tsx
    │   ├── lib/
    │   │   └── task-api.ts
    │   └── types/
    │       └── task.ts
    ├── .env.local
    ├── package.json
    └── tsconfig.json
```

The `.env.local` file is excluded from Git and must be created locally.

## Running the Backend

Start the Week 8 SQL Server and API containers:

```bash
cd ../Week_8
docker compose up sqlserver api
```

Verify the API:

```text
http://localhost:8080/api/tasks
```

## Running the Next.js Portal

Open another terminal:

```bash
cd Week_9/task-manager-portal
npm install
```

Create `.env.local`:

```env
NEXT_PUBLIC_API_URL=http://localhost:8080/api/tasks
```

Start the development server:

```bash
npm run dev
```

Open:

```text
http://localhost:3000
```

## Production Build

To validate the production build:

```bash
npm run build
npm start
```

## Testing Completed

The following functionality was tested:

- Retrieving tasks from the ASP.NET Core API
- Creating a valid task
- Displaying client-side validation errors
- Editing a task
- Marking a task as completed
- Deleting a task after confirmation
- Sorting Grid columns
- Filtering Grid data
- Changing Grid pages
- Updating dashboard statistics
- Displaying API error messages

## Learning Outcomes

This project provided practical experience with:

- Next.js App Router
- Server Components
- Client Components
- Server-side rendering
- Client-side hydration
- TypeScript
- Enterprise component libraries
- KendoReact
- REST API integration
- Client-side validation
- AI-assisted software development
