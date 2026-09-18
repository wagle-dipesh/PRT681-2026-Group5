"use client";

import { useEffect, useState } from "react";
import type { FormEvent } from "react";

import { Button } from "@progress/kendo-react-buttons";
import {
  TextArea,
  TextBox,
} from "@progress/kendo-react-inputs";

import type {
  Task,
  TaskRequest,
} from "@/src/types/task";

interface TaskFormProps {
  editingTask: Task | null;
  saving: boolean;
  onSave: (task: TaskRequest) => Promise<boolean>;
  onCancel: () => void;
}

export default function TaskForm({
  editingTask,
  saving,
  onSave,
  onCancel,
}: TaskFormProps) {
  const [title, setTitle] = useState("");
  const [description, setDescription] = useState("");
  const [isCompleted, setIsCompleted] =
    useState(false);

  const [titleError, setTitleError] = useState("");
  const [descriptionError, setDescriptionError] =
    useState("");

  const resetFields = () => {
    setTitle("");
    setDescription("");
    setIsCompleted(false);
    setTitleError("");
    setDescriptionError("");
  };

  useEffect(() => {
    if (editingTask) {
      setTitle(editingTask.title);
      setDescription(editingTask.description ?? "");
      setIsCompleted(editingTask.isCompleted);
      setTitleError("");
      setDescriptionError("");
    } else {
      setTitle("");
      setDescription("");
      setIsCompleted(false);
      setTitleError("");
      setDescriptionError("");
    }
  }, [editingTask]);

  const validateForm = (): boolean => {
    let isValid = true;

    setTitleError("");
    setDescriptionError("");

    const cleanTitle = title.trim();
    const cleanDescription = description.trim();

    if (!cleanTitle) {
      setTitleError("Task title is required.");
      isValid = false;
    } else if (cleanTitle.length < 3) {
      setTitleError(
        "Task title must contain at least 3 characters."
      );
      isValid = false;
    } else if (cleanTitle.length > 200) {
      setTitleError(
        "Task title cannot exceed 200 characters."
      );
      isValid = false;
    }

    if (cleanDescription.length > 1000) {
      setDescriptionError(
        "Description cannot exceed 1000 characters."
      );
      isValid = false;
    }

    return isValid;
  };

  const handleSubmit = async (
    event: FormEvent<HTMLFormElement>
  ) => {
    event.preventDefault();

    if (!validateForm()) {
      return;
    }

    const savedSuccessfully = await onSave({
      title: title.trim(),
      description: description.trim(),
      isCompleted,
    });

    if (savedSuccessfully) {
      resetFields();
    }
  };

  const handleCancel = () => {
    resetFields();
    onCancel();
  };

  return (
    <section className="mb-8 rounded-xl bg-white p-6 shadow">
      <h2 className="mb-5 text-2xl font-semibold text-slate-900">
        {editingTask ? "Edit Task" : "Create Task"}
      </h2>

      <form
        onSubmit={handleSubmit}
        className="grid gap-5"
        noValidate
      >
        <div>
          <label
            htmlFor="task-title"
            className="mb-2 block font-medium text-slate-700"
          >
            Task title
          </label>

          <TextBox
            id="task-title"
            value={title}
            onChange={(event) =>
              setTitle(String(event.value ?? ""))
            }
            placeholder="Enter the task title"
            valid={!titleError}
            disabled={saving}
          />

          {titleError && (
            <p className="mt-1 text-sm text-red-600">
              {titleError}
            </p>
          )}
        </div>

        <div>
          <label
            htmlFor="task-description"
            className="mb-2 block font-medium text-slate-700"
          >
            Description
          </label>

          <TextArea
            id="task-description"
            value={description}
            onChange={(event) =>
              setDescription(String(event.value ?? ""))
            }
            placeholder="Enter the task description"
            rows={4}
            valid={!descriptionError}
            disabled={saving}
          />

          <div className="mt-1 flex justify-between text-sm">
            {descriptionError ? (
              <p className="text-red-600">
                {descriptionError}
              </p>
            ) : (
              <span />
            )}

            <span className="text-gray-500">
              {description.length}/1000
            </span>
          </div>
        </div>

        {editingTask && (
          <label className="flex items-center gap-3 text-slate-700">
            <input
              type="checkbox"
              checked={isCompleted}
              onChange={(event) =>
                setIsCompleted(event.target.checked)
              }
              disabled={saving}
              className="h-4 w-4"
            />

            <span>Mark this task as completed</span>
          </label>
        )}

        <div className="flex flex-wrap gap-3">
          <Button
            type="submit"
            themeColor="primary"
            disabled={saving}
          >
            {saving
              ? "Saving..."
              : editingTask
                ? "Update Task"
                : "Create Task"}
          </Button>

          {editingTask && (
            <Button
              type="button"
              fillMode="outline"
              disabled={saving}
              onClick={handleCancel}
            >
              Cancel
            </Button>
          )}
        </div>
      </form>
    </section>
  );
}