import { Employee, EmployeeInput } from "./types";

// The address of our ASP.NET Core Web API (set in .env.local).
const API_BASE = process.env.NEXT_PUBLIC_API_URL ?? "http://localhost:5184";

// Thrown when the API rejects our data (a 400 Bad Request with a list of
// error messages), so the form can show them to the user.
export class ApiValidationError extends Error {
  constructor(public errors: string[]) {
    super("Validation failed");
  }
}

async function handleResponse<T>(res: Response): Promise<T> {
  if (res.status === 400) {
    const body = await res.json();
    throw new ApiValidationError(body.errors ?? ["Something was invalid."]);
  }
  if (!res.ok) {
    throw new Error(`Request failed with status ${res.status}`);
  }
  if (res.status === 204) return undefined as T; // no content (used by delete)
  return res.json();
}

export async function getEmployees(): Promise<Employee[]> {
  const res = await fetch(`${API_BASE}/api/employees`, { cache: "no-store" });
  return handleResponse<Employee[]>(res);
}

export async function createEmployee(data: EmployeeInput): Promise<Employee> {
  const res = await fetch(`${API_BASE}/api/employees`, {
    method: "POST",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse<Employee>(res);
}

export async function updateEmployee(id: number, data: EmployeeInput): Promise<Employee> {
  const res = await fetch(`${API_BASE}/api/employees/${id}`, {
    method: "PUT",
    headers: { "Content-Type": "application/json" },
    body: JSON.stringify(data),
  });
  return handleResponse<Employee>(res);
}

export async function deleteEmployee(id: number): Promise<void> {
  const res = await fetch(`${API_BASE}/api/employees/${id}`, { method: "DELETE" });
  return handleResponse<void>(res);
}
