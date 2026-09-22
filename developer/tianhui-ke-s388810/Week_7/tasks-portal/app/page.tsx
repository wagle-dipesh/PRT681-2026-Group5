import TasksGrid from "@/components/TasksGrid";

// The API URL used here runs on the Next.js SERVER during SSR (not in the browser).
// Since dev server and API run on the same machine, "localhost" resolves correctly here.
const API_URL = process.env.NEXT_PUBLIC_API_URL || "http://localhost:8080/api/tasks";

export type Task = {
  id: number;
  title: string;
  isDone: boolean;
};

async function getTasks(): Promise<Task[]> {
  try {
    // no-store: always fetch fresh data server-side rather than using Next's
    // default fetch cache, since this is a live task list, not static content.
    const res = await fetch(API_URL, { cache: "no-store" });
    if (!res.ok) return [];
    return res.json();
  } catch {
    // API might not be running yet — render an empty state rather than crashing
    // the whole page render.
    return [];
  }
}

// This is a React Server Component — it runs on the server at request time
// (Server-Side Rendering, SSR) and fetches the initial task list BEFORE any
// HTML reaches the browser. The client then "hydrates" this HTML with the
// interactive KendoReact Grid below.
export default async function Page() {
  const initialTasks = await getTasks();

  return (
    <main className="page-wrap">
      <h1>Tasks Management Portal</h1>
      <p className="subtitle">
        Server-rendered (SSR) task list, hydrated with an interactive KendoReact grid below.
      </p>
      <div className="grid-card">
        <TasksGrid initialTasks={initialTasks} apiUrl={API_URL} />
      </div>
    </main>
  );
}
