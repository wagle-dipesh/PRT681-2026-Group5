import React, { useState } from 'react';
import './TaskForm.css';

// This component handles creating new tasks

function TaskForm({ onAddTask }) {
  // STATE - Store form input values
  const [title, setTitle] = useState('');
  const [description, setDescription] = useState('');
  const [isSubmitting, setIsSubmitting] = useState(false);

  // HANDLE FORM SUBMISSION
  const handleSubmit = async (e) => {
    e.preventDefault(); // Don't reload the page

    // Validate that title is not empty
    if (!title.trim()) {
      alert('⚠️ Title is required!');
      return;
    }

    setIsSubmitting(true);

    // Create the task object with the form data
    const newTask = {
      title: title.trim(),
      description: description.trim() || null,
      isCompleted: false,
    };

    try {
      // Call the parent component's function to add the task
      await onAddTask(newTask);

      // Clear the form after successful submission
      setTitle('');
      setDescription('');
      alert('Task added successfully!');
    } catch (err) {
      alert('Failed to add task: ' + err.message);
    } finally {
      setIsSubmitting(false);
    }
  };

  // RENDER - Show the form
  return (
    <form onSubmit={handleSubmit} className="task-form">
      <h2>➕ Add New Task</h2>

      {/* Title Input */}
      <div className="form-group">
        <label htmlFor="title">Task Title *</label>
        <input
          type="text"
          id="title"
          value={title}
          onChange={(e) => setTitle(e.target.value)}
          placeholder="What do you need to do?"
          required
          disabled={isSubmitting}
          maxLength="100"
        />
        <small>{title.length}/100 characters</small>
      </div>

      {/* Description Input */}
      <div className="form-group">
        <label htmlFor="description">Description (Optional)</label>
        <textarea
          id="description"
          value={description}
          onChange={(e) => setDescription(e.target.value)}
          placeholder="Add more details about your task..."
          disabled={isSubmitting}
          maxLength="500"
          rows="3"
        />
        <small>{description.length}/500 characters</small>
      </div>

      {/* Submit Button */}
      <button
        type="submit"
        className="submit-btn"
        disabled={isSubmitting}
      >
        {isSubmitting ? '⏳ Adding...' : '✅ Add Task'}
      </button>
    </form>
  );
}

export default TaskForm;
