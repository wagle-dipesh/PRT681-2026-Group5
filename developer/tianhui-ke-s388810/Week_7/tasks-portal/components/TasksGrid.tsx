"use client";

import { useState } from "react";
import { Grid, GridColumn, GridItemChangeEvent, GridToolbar } from "@progress/kendo-react-grid";
import { Input } from "@progress/kendo-react-inputs";
import { Button } from "@progress/kendo-react-buttons";
import type { Task } from "../app/page";

type Props = {
  initialTasks: Task[];
  apiUrl: string;
};

// This is a Client Component ("use client") — unlike the Server Component in
// page.tsx, this one runs in the BROWSER, so `apiUrl` here is fetched from the
// client's own network context, not the server's. Since both point at the
// same localhost API in local dev, it works the same either way — but this
// distinction matters once the API moves somewhere the browser can't reach
// directly (e.g. a private network the Next.js server can see but the
// visitor's browser can't).
export default function TasksGrid({ initialTasks, apiUrl }: Props) {
  const [tasks, setTasks] = useState<Task[]>(initialTasks);
  const [newTitle, setNewTitle] = useState("");
  const [error, setError] = useState<string | null>(null);

  async function refresh() {
    try {
      const res = await fetch(apiUrl);
      if (!res.ok) throw new Error();
      setTasks(await res.json());
    } catch {
      setError("Couldn't refresh tasks from the API.");
    }
  }

  async function addTask() {
    if (!newTitle.trim()) return;
    try {
      const res = await fetch(apiUrl, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title: newTitle, isDone: false }),
      });
      if (!res.ok) throw new Error();
      setNewTitle("");
      await refresh();
    } catch {
      setError("Failed to add task.");
    }
  }

  async function deleteTask(id: number) {
    try {
      const res = await fetch(`${apiUrl}/${id}`, { method: "DELETE" });
      if (!res.ok) throw new Error();
      await refresh();
    } catch {
      setError("Failed to delete task.");
    }
  }

  // KendoReact Grid's inline-edit pattern: itemChange updates the local copy
  // immediately (so the UI feels responsive), then persists the change to the
  // API via PUT. This is the standard "optimistic update" pattern their docs
  // recommend for editable grids.
  async function handleItemChange(event: GridItemChangeEvent) {
    const updated = tasks.map((t) =>
      t.id === event.dataItem.id ? { ...t, [event.field as string]: event.value } : t
    );
    setTasks(updated);

    const changedTask = updated.find((t) => t.id === event.dataItem.id);
    if (!changedTask) return;

    try {
      const res = await fetch(`${apiUrl}/${changedTask.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify(changedTask),
      });
      if (!res.ok) throw new Error();
    } catch {
      setError("Failed to save change — refresh to see the last saved state.");
    }
  }

  return (
    <div>
      {error && <div style={{ color: "#a32d2d", marginBottom: 12 }}>{error}</div>}

      <Grid
        data={tasks}
        editField="inEdit"
        onItemChange={handleItemChange}
        style={{ maxHeight: 420 }}
      >
        <GridToolbar>
          <Input
            placeholder="New task title"
            value={newTitle}
            onChange={(e) => setNewTitle(String(e.value ?? ""))}
          />
          <Button themeColor="primary" onClick={addTask}>
            Add
          </Button>
        </GridToolbar>
        <GridColumn field="id" title="ID" width="70px" editable={false} />
        <GridColumn field="title" title="Title" editor="text" />
        <GridColumn field="isDone" title="Done" editor="boolean" width="90px" />
        <GridColumn
          title="Actions"
          width="100px"
          cell={(props) => (
            <td>
              <Button
                fillMode="flat"
                themeColor="error"
                onClick={() => deleteTask(props.dataItem.id)}
              >
                Delete
              </Button>
            </td>
          )}
        />
      </Grid>
    </div>
  );
}
