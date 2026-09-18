import TaskGrid from "@/src/components/TaskGrid";
import { getTasks } from "@/src/lib/task-api";
import { Task } from "@/src/types/task";

export const dynamic = "force-dynamic";

export default async function Home() {
  let tasks: Task[] = [];
  let errorMessage = "";

  try {
    tasks = await getTasks();
  } catch (error) {
    errorMessage =
      error instanceof Error
        ? error.message
        : "The task service is currently unavailable.";
  }

  const completedTasks = tasks.filter((task) => task.isCompleted).length;

  const pendingTasks = tasks.length - completedTasks;

  return (
    <main className="min-h-screen bg-gray-100 px-4 py-8 sm:px-8">
      <div className="mx-auto max-w-7xl">
        <header className="mb-8 rounded-xl bg-slate-900 p-6 text-white shadow-lg">
          <h1 className="text-3xl font-bold sm:text-4xl">
            Task Management Portal
          </h1>
        </header>

        <section className="mb-8 grid gap-4 sm:grid-cols-3">
          <article className="rounded-xl bg-white p-5 shadow">
            <p className="text-sm font-medium text-gray-500">Total Tasks</p>

            <p className="mt-2 text-3xl font-bold text-slate-900">
              {tasks.length}
            </p>
          </article>

          <article className="rounded-xl bg-white p-5 shadow">
            <p className="text-sm font-medium text-gray-500">Completed</p>

            <p className="mt-2 text-3xl font-bold text-green-600">
              {completedTasks}
            </p>
          </article>

          <article className="rounded-xl bg-white p-5 shadow">
            <p className="text-sm font-medium text-gray-500">Pending</p>

            <p className="mt-2 text-3xl font-bold text-amber-600">
              {pendingTasks}
            </p>
          </article>
        </section>

        {errorMessage ? (
          <div
            role="alert"
            className="rounded-lg border border-red-300 bg-red-50 p-4 text-red-700"
          >
            <h2 className="font-semibold">Unable to load tasks</h2>

            <p>{errorMessage}</p>

            <p className="mt-2 text-sm">
              Confirm that the ASP.NET Core API is running on port 8080.
            </p>
          </div>
        ) : (
          <TaskGrid initialTasks={tasks} />
        )}
      </div>
    </main>
  );
}
