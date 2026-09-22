export interface Employee {
  id: number;
  fullName: string;
  email: string;
  department: string;
  jobTitle: string;
  salary: number;
  hireDate: string;
  isActive: boolean;
}

export type EmployeeInput = Omit<Employee, "id">;

export const DEPARTMENTS = [
  "Engineering",
  "Sales",
  "Marketing",
  "HR",
  "Finance",
  "Operations",
] as const;
