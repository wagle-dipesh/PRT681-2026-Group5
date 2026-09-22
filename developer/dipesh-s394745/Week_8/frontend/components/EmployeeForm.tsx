"use client";

import { useState, FormEvent } from "react";
import { Input, NumericTextBox, Checkbox } from "@progress/kendo-react-inputs";
import { DropDownList } from "@progress/kendo-react-dropdowns";
import { DatePicker } from "@progress/kendo-react-dateinputs";
import { Button } from "@progress/kendo-react-buttons";
import { Dialog, DialogActionsBar } from "@progress/kendo-react-dialogs";
import { Employee, EmployeeInput, DEPARTMENTS } from "@/lib/types";

interface Props {
  employee?: Employee | null; // an Employee when editing, null when adding a new one
  onSubmit: (data: EmployeeInput) => Promise<void>;
  onCancel: () => void;
  serverErrors?: string[];
}

// A simple Add/Edit form. Every field has its own useState, and we validate
// by hand before calling onSubmit - no extra form library needed.
export default function EmployeeForm({ employee, onSubmit, onCancel, serverErrors = [] }: Props) {
  const isEdit = !!employee;

  const [fullName, setFullName] = useState(employee?.fullName ?? "");
  const [email, setEmail] = useState(employee?.email ?? "");
  const [department, setDepartment] = useState(employee?.department ?? DEPARTMENTS[0]);
  const [jobTitle, setJobTitle] = useState(employee?.jobTitle ?? "");
  const [salary, setSalary] = useState(employee?.salary ?? 0);
  const [hireDate, setHireDate] = useState(employee ? new Date(employee.hireDate) : new Date());
  const [isActive, setIsActive] = useState(employee?.isActive ?? true);

  const [errors, setErrors] = useState<string[]>([]);
  const [saving, setSaving] = useState(false);

  function validate(): string[] {
    const problems: string[] = [];
    if (!fullName.trim()) problems.push("Full name is required.");
    if (!email.includes("@")) problems.push("Enter a valid email address.");
    if (!jobTitle.trim()) problems.push("Job title is required.");
    if (salary < 0) problems.push("Salary cannot be negative.");
    return problems;
  }

  async function handleSubmit(e: FormEvent) {
    e.preventDefault();

    const problems = validate();
    setErrors(problems);
    if (problems.length > 0) return; // stop here - don't call the API yet

    setSaving(true);
    await onSubmit({
      fullName,
      email,
      department,
      jobTitle,
      salary,
      hireDate: hireDate.toISOString(),
      isActive,
    });
    setSaving(false);
  }

  const allErrors = [...errors, ...serverErrors];

  return (
    <Dialog title={isEdit ? "Edit Employee" : "Add Employee"} onClose={onCancel} width={420}>
      <form onSubmit={handleSubmit} className="flex flex-col gap-3">
        <div>
          <label className="text-sm font-medium">Full name</label>
          <Input value={fullName} onChange={(e) => setFullName(String(e.value ?? ""))} className="w-full" />
        </div>

        <div>
          <label className="text-sm font-medium">Email</label>
          <Input value={email} onChange={(e) => setEmail(String(e.value ?? ""))} className="w-full" />
        </div>

        <div>
          <label className="text-sm font-medium">Department</label>
          <DropDownList
            data={[...DEPARTMENTS]}
            value={department}
            onChange={(e) => setDepartment(e.value)}
            className="w-full"
          />
        </div>

        <div>
          <label className="text-sm font-medium">Job title</label>
          <Input value={jobTitle} onChange={(e) => setJobTitle(String(e.value ?? ""))} className="w-full" />
        </div>

        <div>
          <label className="text-sm font-medium">Annual salary</label>
          <NumericTextBox
            value={salary}
            onChange={(e) => setSalary(e.value ?? 0)}
            format="c0"
            min={0}
            className="w-full"
          />
        </div>

        <div>
          <label className="text-sm font-medium">Hire date</label>
          <DatePicker value={hireDate} onChange={(e) => setHireDate(e.value ?? new Date())} className="w-full" />
        </div>

        <Checkbox
          checked={isActive}
          onChange={(e) => setIsActive(Boolean(e.value))}
          label="Active employee"
        />

        {allErrors.length > 0 && (
          <ul className="text-red-600 text-sm list-disc pl-5">
            {allErrors.map((msg) => (
              <li key={msg}>{msg}</li>
            ))}
          </ul>
        )}

        <DialogActionsBar>
          <Button type="button" onClick={onCancel}>
            Cancel
          </Button>
          <Button type="submit" themeColor="primary" disabled={saving}>
            {isEdit ? "Save changes" : "Add employee"}
          </Button>
        </DialogActionsBar>
      </form>
    </Dialog>
  );
}
