import React, { useState } from 'react';
import './TaskItem.css';

// TASK ITEM COMPONENT

function TaskItem({ task, onDelete, onToggleComplete }) {
  const [isDeleting, setIsDeleting] = useState(false);
  const [isUpdating, setIsUpdating] = useState(false);

  // HANDLE DELETE BUTTON CLICK
  const handleDelete = async () => {
    if (window.confirm('Are you sure you want to delete this task?')) {
      setIsDeleting(true);
      try {
        await onDelete(task.id);
      } catch (err) {
        console.error('Error deleting task:', err);
        setIsDeleting(false);
      }
    }
  };

  // HANDLE COMPLETE/INCOMPLETE TOGGLE
  const handleToggle = async () => {
    setIsUpdating(true);
    try {
      await onToggleComplete(task);
    } catch (err) {
      console.error('Error updating task:', err);
      setIsUpdating(false);
    } finally {
      setIsUpdating(false);
    }
  };

  // FORMAT DATE (Make it human-readable)
  const formatDate = (dateString) => {
    const date = new Date(dateString);
    return date.toLocaleDateString('en-US', {
      month: 'short',
      day: 'numeric',
      year: 'numeric',
      hour: '2-digit',
      minute: '2-digit',
    });
  };

  // RENDER - Display the task
  return (
    <div className={`task-item ${task.isCompleted ? 'completed' : ''}`}>
      {/* Left side - Checkbox and task info */}
      <div className="task-content">
        <input
          type="checkbox"
          checked={task.isCompleted}
          onChange={handleToggle}
          disabled={isUpdating}
          className="task-checkbox"
        />

        <div className="task-info">
          <h3 className="task-title">{task.title}</h3>
          {task.description && (
            <p className="task-description">{task.description}</p>
          )}
          <p className="task-date">📅 {formatDate(task.createdAt)}</p>
        </div>
      </div>

      {/* Right side - Status badge and delete button */}
      <div className="task-actions">
        <span className={`task-badge ${task.isCompleted ? 'completed' : 'pending'}`}>
          {task.isCompleted ? '✅ Done' : '⏳ Pending'}
        </span>

        <button
          onClick={handleDelete}
          disabled={isDeleting}
          className="delete-btn"
          title="Delete this task"
        >
          {isDeleting ? '🗑️' : '❌'}
        </button>
      </div>
    </div>
  );
}

export default TaskItem;
