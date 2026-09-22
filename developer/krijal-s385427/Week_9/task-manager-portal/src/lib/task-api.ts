import { Task, TaskRequest } from "../types/task";


const API_URL =
  process.env.NEXT_PUBLIC_API_URL ??
  "http://localhost:8080/api/tasks";

export async function getTasks(): Promise<Task[]> {
  const response = await fetch(API_URL, {
    cache: "no-store",
  });

  if (!response.ok) {
    throw new Error("Unable to retrieve tasks.");
  }

  return response.json();
}

export async function createTask(task: TaskRequest): Promise<void> {
  const response = await fetch(API_URL, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(task),
  });

  if (!response.ok) {
    throw new Error("Unable to create the task.");
  }
}

export async function updateTask(
  id: number,
  task: TaskRequest
): Promise<void> {
  const response = await fetch(`${API_URL}/${id}`, {
    method: "PUT",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(task),
  });

  if (!response.ok) {
    throw new Error("Unable to update the task.");
  }
}

export async function deleteTask(id: number): Promise<void> {
  const response = await fetch(`${API_URL}/${id}`, {
    method: "DELETE",
  });

  if (!response.ok) {
    throw new Error("Unable to delete the task.");
  }
}