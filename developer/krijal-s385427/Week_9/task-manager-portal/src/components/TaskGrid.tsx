"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";

import { Grid, GridColumn } from "@progress/kendo-react-grid";

import type {
  GridCustomCellProps,
  GridDataStateChangeEvent,
} from "@progress/kendo-react-grid";

import { Button } from "@progress/kendo-react-buttons";
import { process } from "@progress/kendo-data-query";
import type { State } from "@progress/kendo-data-query";

import TaskForm from "@/src/components/TaskForm";

import {
  createTask,
  deleteTask,
  getTasks,
  updateTask,
} from "@/src/lib/task-api";

import type { Task, TaskRequest } from "@/src/types/task";

interface TaskGridProps {
  initialTasks: Task[];
}

export default function TaskGrid({ initialTasks }: TaskGridProps) {
  const router = useRouter();

  const [tasks, setTasks] = useState<Task[]>(initialTasks);

  const [editingTask, setEditingTask] = useState<Task | null>(null);

  const [saving, setSaving] = useState(false);
  const [deletingId, setDeletingId] = useState<number | null>(null);

  const [errorMessage, setErrorMessage] = useState("");
  const [successMessage, setSuccessMessage] = useState("");

  const [dataState, setDataState] = useState<State>({
    skip: 0,
    take: 5,
    sort: [],
    filter: undefined,
  });

  const refreshTasks = async () => {
    const refreshedTasks = await getTasks();

    setTasks(refreshedTasks);

    // Update the statistics rendered by the Server Component.
    router.refresh();
  };

  const handleSave = async (task: TaskRequest): Promise<boolean> => {
    try {
      setSaving(true);
      setErrorMessage("");
      setSuccessMessage("");

      const wasEditing = editingTask !== null;

      if (editingTask) {
        await updateTask(editingTask.id, task);
      } else {
        await createTask(task);
      }

      setEditingTask(null);

      await refreshTasks();

      setSuccessMessage(
        wasEditing
          ? "Task updated successfully."
          : "Task created successfully.",
      );

      return true;
    } catch (error) {
      const message =
        error instanceof Error ? error.message : "Unable to save the task.";

      setErrorMessage(message);

      return false;
    } finally {
      setSaving(false);
    }
  };

  const handleDelete = async (task: Task) => {
    const confirmed = window.confirm(
      `Are you sure you want to delete "${task.title}"?`,
    );

    if (!confirmed) {
      return;
    }

    try {
      setDeletingId(task.id);
      setErrorMessage("");
      setSuccessMessage("");

      await deleteTask(task.id);

      if (editingTask?.id === task.id) {
        setEditingTask(null);
      }

      await refreshTasks();

      setSuccessMessage("Task deleted successfully.");
    } catch (error) {
      const message =
        error instanceof Error ? error.message : "Unable to delete the task.";

      setErrorMessage(message);
    } finally {
      setDeletingId(null);
    }
  };

  const handleEdit = (task: Task) => {
    setEditingTask(task);
    setErrorMessage("");
    setSuccessMessage("");

    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  };

  const handleCancel = () => {
    setEditingTask(null);
    setErrorMessage("");
    setSuccessMessage("");
  };

  const handleDataStateChange = (event: GridDataStateChangeEvent) => {
    setDataState(event.dataState);
  };

  const ActionCell = (props: GridCustomCellProps) => {
    const task = props.dataItem as Task;
    const isDeleting = deletingId === task.id;

    return (
      <td {...props.tdProps}>
        <div className="flex gap-2">
          <Button
            size="small"
            themeColor="primary"
            fillMode="outline"
            disabled={isDeleting}
            onClick={() => handleEdit(task)}
          >
            Edit
          </Button>

          <Button
            size="small"
            themeColor="error"
            disabled={isDeleting}
            onClick={() => handleDelete(task)}
          >
            {isDeleting ? "Deleting..." : "Delete"}
          </Button>
        </div>
      </td>
    );
  };

  return (
    <>
      <TaskForm
        editingTask={editingTask}
        saving={saving}
        onSave={handleSave}
        onCancel={handleCancel}
      />

      {successMessage && (
        <div
          role="status"
          className="mb-6 rounded-lg border border-green-300 bg-green-50 p-4 text-green-700"
        >
          {successMessage}
        </div>
      )}

      {errorMessage && (
        <div
          role="alert"
          className="mb-6 rounded-lg border border-red-300 bg-red-50 p-4 text-red-700"
        >
          {errorMessage}
        </div>
      )}

      <section>
        <div className="mb-4">
          <h2 className="text-2xl font-semibold text-slate-900">
            Task Management
          </h2>

          <p className="text-gray-600">
            Create, search, sort, edit and delete tasks.
          </p>
        </div>

        {tasks.length === 0 ? (
          <div className="rounded-lg bg-white p-8 text-center shadow">
            <p className="text-gray-600">
              No tasks are currently available. Create your first task using the
              form above.
            </p>
          </div>
        ) : (
          <div className="overflow-x-auto rounded-lg bg-white shadow">
            <Grid
              data={process(tasks, dataState)}
              dataItemKey="id"
              {...dataState}
              pageable
              sortable
              filterable
              onDataStateChange={handleDataStateChange}
              style={{
                minWidth: "950px",
              }}
            >
              {/* <GridColumn field="id" title="ID" width="70px" /> */}

              <GridColumn field="title" title="Task Title" width="200px" />

              <GridColumn field="description" title="Description" />

              <GridColumn
                field="isCompleted"
                title="Completed"
                filter="boolean"
                width="140px"
              />

              <GridColumn field="createdAt" title="Created" width="210px" />

              <GridColumn
                title="Actions"
                width="190px"
                sortable={false}
                filterable={false}
                cells={{
                  data: ActionCell,
                }}
              />
            </Grid>
          </div>
        )}
      </section>
    </>
  );
}
