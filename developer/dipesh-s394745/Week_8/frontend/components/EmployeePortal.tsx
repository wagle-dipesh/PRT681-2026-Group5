"use client";

import { useEffect, useState } from "react";
import { Grid, GridColumn, GridToolbar, GridCustomCellProps } from "@progress/kendo-react-grid";
import { Button } from "@progress/kendo-react-buttons";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Input } from "@progress/kendo-react-inputs";
import { Employee, EmployeeInput } from "@/lib/types";
import {
  getEmployees,
  createEmployee,
  updateEmployee,
  deleteEmployee,
  ApiValidationError,
} from "@/lib/api";
import EmployeeForm from "./EmployeeForm";

// Shows the Active/Inactive badge in the "Status" column.
function StatusCell(props: GridCustomCellProps) {
  const employee = props.dataItem as Employee;
  return (
    <td {...props.tdProps}>
      <span
        className={`px-2 py-1 rounded-full text-xs font-medium ${
          employee.isActive ? "bg-green-100 text-green-700" : "bg-gray-200 text-gray-600"
        }`}
      >
        {employee.isActive ? "Active" : "Inactive"}
      </span>
    </td>
  );
}

export default function EmployeePortal() {
  const [employees, setEmployees] = useState<Employee[]>([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState<string | null>(null);
  const [search, setSearch] = useState("");

  // editing === undefined -> dialog closed
  // editing === null      -> dialog open, adding a new employee
  // editing === Employee  -> dialog open, editing that employee
  const [editing, setEditing] = useState<Employee | null | undefined>(undefined);
  const [deleting, setDeleting] = useState<Employee | null>(null);
  const [serverErrors, setServerErrors] = useState<string[]>([]);

  async function loadEmployees() {
    setLoading(true);
    setError(null);
    try {
      setEmployees(await getEmployees());
    } catch {
      setError(
        "Could not reach the ASP.NET Core API at http://localhost:5184. Make sure `dotnet run` is running in backend/EmployeeApi."
      );
    } finally {
      setLoading(false);
    }
  }

  useEffect(() => {
    loadEmployees();
  }, []);

  const visibleEmployees = employees.filter((e) =>
    `${e.fullName} ${e.email} ${e.department} ${e.jobTitle}`
      .toLowerCase()
      .includes(search.toLowerCase())
  );

  // The Grid's date formatting needs a real Date object, but the API gives us
  // a plain string - so we convert it here, just for display.
  const gridData = visibleEmployees.map((e) => ({ ...e, hireDate: new Date(e.hireDate) }));

  async function handleSave(data: EmployeeInput) {
    setServerErrors([]);
    try {
      if (editing) {
        const updated = await updateEmployee(editing.id, data);
        setEmployees((prev) => prev.map((e) => (e.id === updated.id ? updated : e)));
      } else {
        const created = await createEmployee(data);
        setEmployees((prev) => [...prev, created]);
      }
      setEditing(undefined);
    } catch (err) {
      if (err instanceof ApiValidationError) {
        setServerErrors(err.errors);
      } else {
        setError("Failed to save employee. Check the API console for details.");
      }
    }
  }

  async function confirmDelete() {
    if (!deleting) return;
    try {
      await deleteEmployee(deleting.id);
      setEmployees((prev) => prev.filter((e) => e.id !== deleting.id));
    } catch {
      setError("Failed to delete employee.");
    } finally {
      setDeleting(null);
    }
  }

  function ActionsCell(props: GridCustomCellProps) {
    const employee = props.dataItem as Employee;
    return (
      <td {...props.tdProps} className="flex gap-2">
        <Button size="small" onClick={() => setEditing(employee)}>
          Edit
        </Button>
        <Button size="small" themeColor="error" onClick={() => setDeleting(employee)}>
          Delete
        </Button>
      </td>
    );
  }

  return (
    <div className="w-full max-w-6xl mx-auto p-6">
      <div className="flex items-center justify-between mb-4">
        <div>
          <h1 className="text-2xl font-semibold">Employee Management Portal</h1>
          <p className="text-sm text-gray-500">
            Next.js + Kendo UI React Grid, backed by an ASP.NET Core Web API
          </p>
        </div>
        <Button themeColor="primary" onClick={() => setEditing(null)}>
          + Add Employee
        </Button>
      </div>

      {error && (
        <div className="mb-4 rounded-md border border-red-300 bg-red-50 text-red-700 px-4 py-3 text-sm">
          {error}
        </div>
      )}

      {loading && <p className="mb-2 text-sm text-gray-500">Loading employees…</p>}

      <Grid data={gridData} style={{ height: "560px" }} sortable pageable={{ pageSizes: [5, 10, 20] }}>
        <GridToolbar>
          <Input
            placeholder="Search employees..."
            value={search}
            onChange={(e) => setSearch(String(e.value ?? ""))}
          />
        </GridToolbar>
        <GridColumn field="fullName" title="Name" width="180px" />
        <GridColumn field="email" title="Email" width="220px" />
        <GridColumn field="department" title="Department" width="140px" />
        <GridColumn field="jobTitle" title="Title" width="160px" />
        <GridColumn field="salary" title="Salary" width="120px" format="{0:c0}" />
        <GridColumn field="hireDate" title="Hire date" width="120px" format="{0:d}" />
        <GridColumn field="isActive" title="Status" width="100px" cells={{ data: StatusCell }} />
        <GridColumn title="Actions" width="160px" cells={{ data: ActionsCell }} sortable={false} />
      </Grid>

      {editing !== undefined && (
        <EmployeeForm
          employee={editing}
          serverErrors={serverErrors}
          onCancel={() => {
            setEditing(undefined);
            setServerErrors([]);
          }}
          onSubmit={handleSave}
        />
      )}

      {deleting && (
        <Dialog title="Confirm delete" onClose={() => setDeleting(null)}>
          <p>
            Are you sure you want to delete <strong>{deleting.fullName}</strong>?
          </p>
          <DialogActionsBar>
            <Button onClick={() => setDeleting(null)}>Cancel</Button>
            <Button themeColor="error" onClick={confirmDelete}>
              Delete
            </Button>
          </DialogActionsBar>
        </Dialog>
      )}
    </div>
  );
}
