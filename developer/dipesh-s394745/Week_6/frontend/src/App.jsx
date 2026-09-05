import React, { useState, useEffect } from 'react';
import TaskForm from './TaskForm';
import TaskItem from './TaskItem';
import './App.css';


function App() {
  // STATE
  const [tasks, setTasks] = useState([]);          
  const [loading, setLoading] = useState(false);    
  const [error, setError] = useState(null);       

  // API URL (where your backend is running)
  const API_URL = 'https://localhost:44366/api/tasks';

  const fetchTasks = async () => {
    setLoading(true);
    setError(null);
    try {
      const response = await fetch(API_URL);
      if (!response.ok) {
        throw new Error('Failed to fetch tasks');
      }
      const data = await response.json();
      setTasks(data);
    } catch (err) {
      setError(err.message);
      console.error('Error fetching tasks:', err);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    fetchTasks(); 
  }, []); 

  // ADD NEW TASK
  const handleAddTask = async (newTask) => {
    try {
      const response = await fetch(API_URL, {
        method: 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify(newTask),
      });

      if (!response.ok) {
        throw new Error('Failed to add task');
      }

      fetchTasks();
    } catch (err) {
      setError(err.message);
      console.error('Error adding task:', err);
    }
  };

  // DELETE A TASK
  const handleDeleteTask = async (taskId) => {
    try {
      const response = await fetch(`${API_URL}/${taskId}`, {
        method: 'DELETE',
      });

      if (!response.ok) {
        throw new Error('Failed to delete task');
      }

      setTasks(tasks.filter(task => task.id !== taskId));
    } catch (err) {
      setError(err.message);
      console.error('Error deleting task:', err);
    }
  };

  // MARK TASK AS COMPLETE/INCOMPLETE
  const handleToggleComplete = async (task) => {
    try {
      const response = await fetch(`${API_URL}/${task.id}`, {
        method: 'PUT',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          ...task,
          isCompleted: !task.isCompleted,
        }),
      });

      if (!response.ok) {
        throw new Error('Failed to update task');
      }

      // Update the task 
      setTasks(tasks.map(t =>
        t.id === task.id ? { ...t, isCompleted: !t.isCompleted } : t
      ));
    } catch (err) {
      setError(err.message);
      console.error('Error updating task:', err);
    }
  };

  // RENDER 
  return (
    <div className="app">
      <header className="app-header">
        <h1>📝 Task Manager</h1>
        <p>A simple CRUD application with React & ASP.NET Core</p>
      </header>

      <main className="app-main">
        {error && (
          <div className="error-message">
            ⚠️ Error: {error}
          </div>
        )}

        {/* Form to add new tasks */}
        <TaskForm onAddTask={handleAddTask} />

        {/* Show loading message while fetching */}
        {loading && <p className="loading">Loading tasks...</p>}

        {/* Show tasks list */}
        <div className="tasks-section">
          <h2>
            My Tasks ({tasks.length})
          </h2>

          {tasks.length === 0 && !loading ? (
            <p className="empty-message">
              No tasks yet! Add one to get started.
            </p>
          ) : (
            <div className="tasks-list">
              {tasks.map(task => (
                <TaskItem
                  key={task.id}
                  task={task}
                  onDelete={handleDeleteTask}
                  onToggleComplete={handleToggleComplete}
                />
              ))}
            </div>
          )}
        </div>
      </main>
    </div>
  );
}

export default App;
