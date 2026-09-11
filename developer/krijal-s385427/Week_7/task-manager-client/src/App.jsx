import { useEffect, useState } from 'react'
import { Button } from '@progress/kendo-react-buttons'
import { TextArea, TextBox } from '@progress/kendo-react-inputs'
import './App.css'

const API_URL = 'http://localhost:5150/api/tasks'

function App() {
  const [tasks, setTasks] = useState([])
  const [title, setTitle] = useState('')
  const [description, setDescription] = useState('')
  const [isCompleted, setIsCompleted] = useState(false)
  const [editingId, setEditingId] = useState(null)
  const [loading, setLoading] = useState(true)
  const [saving, setSaving] = useState(false)
  const [error, setError] = useState('')

  const loadTasks = async () => {
    try {
      setError('')

      const response = await fetch(API_URL)

      if (!response.ok) {
        throw new Error('Unable to retrieve tasks.')
      }

      const data = await response.json()
      setTasks(data)
    } catch (error) {
      setError(error.message)
    } finally {
      setLoading(false)
    }
  }

  useEffect(() => {
    loadTasks()
  }, [])

  const resetForm = () => {
    setTitle('')
    setDescription('')
    setIsCompleted(false)
    setEditingId(null)
  }

  const saveTask = async (event) => {
    event.preventDefault()

    if (!title.trim()) {
      setError('Please enter a task title.')
      return
    }

    try {
      setSaving(true)
      setError('')

      const isEditing = editingId !== null
      const url = isEditing
        ? `${API_URL}/${editingId}`
        : API_URL

      const response = await fetch(url, {
        method: isEditing ? 'PUT' : 'POST',
        headers: {
          'Content-Type': 'application/json',
        },
        body: JSON.stringify({
          title: title.trim(),
          description: description.trim(),
          isCompleted,
        }),
      })

      if (!response.ok) {
        throw new Error(
          isEditing
            ? 'Unable to update the task.'
            : 'Unable to add the task.'
        )
      }

      resetForm()
      await loadTasks()
    } catch (error) {
      setError(error.message)
    } finally {
      setSaving(false)
    }
  }

  const startEditing = (task) => {
    setEditingId(task.id)
    setTitle(task.title)
    setDescription(task.description || '')
    setIsCompleted(task.isCompleted)
    setError('')

    window.scrollTo({
      top: 0,
      behavior: 'smooth',
    })
  }

  const cancelEditing = () => {
    resetForm()
    setError('')
  }

  const deleteTask = async (id) => {
    const confirmed = window.confirm(
      'Are you sure you want to delete this task?'
    )

    if (!confirmed) {
      return
    }

    try {
      setError('')

      const response = await fetch(`${API_URL}/${id}`, {
        method: 'DELETE',
      })

      if (!response.ok) {
        throw new Error('Unable to delete the task.')
      }

      if (editingId === id) {
        resetForm()
      }

      await loadTasks()
    } catch (error) {
      setError(error.message)
    }
  }

  return (
    <main className="app-container">
      <header className="page-header">
        <p className="eyebrow">WEEK 7 PROJECT</p>
        <h1>Task Manager</h1>
        <p>React, KendoReact and ASP.NET Core Web API</p>
      </header>

      <section className="panel">
        <h2>{editingId ? 'Edit task' : 'Add a new task'}</h2>

        <form onSubmit={saveTask} className="task-form">
          <label htmlFor="title">Task title</label>

          <TextBox
            id="title"
            value={title}
            onChange={(event) => setTitle(event.value)}
            placeholder="Enter the task title"
          />

          <label htmlFor="description">Description</label>

          <TextArea
            id="description"
            value={description}
            onChange={(event) => setDescription(event.value)}
            placeholder="Enter the task description"
            rows={4}
          />

          {editingId && (
            <label className="completion-field">
              <input
                type="checkbox"
                checked={isCompleted}
                onChange={(event) =>
                  setIsCompleted(event.target.checked)
                }
              />

              Mark this task as completed
            </label>
          )}

          <div className="form-actions">
            <Button
              type="submit"
              themeColor="primary"
              disabled={saving}
            >
              {saving
                ? 'Saving...'
                : editingId
                  ? 'Update Task'
                  : 'Add Task'}
            </Button>

            {editingId && (
              <Button
                type="button"
                fillMode="outline"
                onClick={cancelEditing}
              >
                Cancel
              </Button>
            )}
          </div>
        </form>

        {error && <p className="error-message">{error}</p>}
      </section>

      <section className="panel">
        <div className="task-heading">
          <h2>All Tasks</h2>
          <span className="task-count">{tasks.length} tasks</span>
        </div>

        {loading ? (
          <p>Loading tasks...</p>
        ) : tasks.length === 0 ? (
          <p>No tasks are currently available.</p>
        ) : (
          <div className="task-list">
            {tasks.map((task) => (
              <article className="task-card" key={task.id}>
                <div className="task-content">
                  <div className="task-title-row">
                    <h3>{task.title}</h3>

                    <span
                      className={
                        task.isCompleted
                          ? 'status completed'
                          : 'status pending'
                      }
                    >
                      {task.isCompleted ? 'Completed' : 'Pending'}
                    </span>
                  </div>

                  <p>{task.description || 'No description provided.'}</p>

                  <small>
                    Created: {new Date(task.createdAt).toLocaleString()}
                  </small>
                </div>

                <div className="task-actions">
                  <Button
                    themeColor="primary"
                    fillMode="outline"
                    onClick={() => startEditing(task)}
                  >
                    Edit
                  </Button>

                  <Button
                    themeColor="error"
                    onClick={() => deleteTask(task.id)}
                  >
                    Delete
                  </Button>
                </div>
              </article>
            ))}
          </div>
        )}
      </section>
    </main>
  )
}

export default App