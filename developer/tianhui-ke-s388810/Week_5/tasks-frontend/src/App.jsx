import { useEffect, useRef, useState } from "react";

const API_URL = "http://localhost:5000/api/tasks"; // update if your API runs on a different port

const DEFAULT_BG_COLOR = "#fde2e7"; // light pink default background
const BG_STORAGE_KEY = "cdu-tasks-custom-bg";

export default function App() {
  const [tasks, setTasks] = useState([]);
  const [title, setTitle] = useState("");
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState(null);
  const [customBg, setCustomBg] = useState(null);
  const fileInputRef = useRef(null);

  // Load any previously-uploaded background from this browser's local storage,
  // so the customisation persists across page reloads (not sent anywhere — purely
  // client-side, per this being a personal styling preference, not app data).
  useEffect(() => {
    const saved = localStorage.getItem(BG_STORAGE_KEY);
    if (saved) setCustomBg(saved);
  }, []);

  useEffect(() => {
    fetchTasks();
  }, []);

  async function fetchTasks() {
    setLoading(true);
    setError(null);
    try {
      const res = await fetch(API_URL);
      if (!res.ok) throw new Error(`Server returned ${res.status}`);
      const data = await res.json();
      setTasks(data);
    } catch (err) {
      setError("Couldn't load tasks. Is the API running?");
    } finally {
      setLoading(false);
    }
  }

  async function addTask(e) {
    e.preventDefault();
    if (!title.trim()) return;

    try {
      const res = await fetch(API_URL, {
        method: "POST",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ title, isDone: false }),
      });
      if (!res.ok) {
        const body = await res.json().catch(() => ({}));
        throw new Error(body.error || "Failed to add task");
      }
      setTitle("");
      fetchTasks();
    } catch (err) {
      setError(err.message);
    }
  }

  async function toggleDone(task) {
    try {
      const res = await fetch(`${API_URL}/${task.id}`, {
        method: "PUT",
        headers: { "Content-Type": "application/json" },
        body: JSON.stringify({ id: task.id, title: task.title, isDone: !task.isDone }),
      });
      if (!res.ok) throw new Error("Failed to update task");
      fetchTasks();
    } catch (err) {
      setError(err.message);
    }
  }

  async function deleteTask(id) {
    try {
      const res = await fetch(`${API_URL}/${id}`, { method: "DELETE" });
      if (!res.ok) throw new Error(`Failed to delete task ${id}`);
      fetchTasks();
    } catch (err) {
      setError(err.message);
    }
  }

  function handleBackgroundUpload(e) {
    const file = e.target.files?.[0];
    if (!file) return;

    const reader = new FileReader();
    reader.onload = () => {
      const dataUrl = reader.result;
      setCustomBg(dataUrl);
      localStorage.setItem(BG_STORAGE_KEY, dataUrl);
    };
    reader.readAsDataURL(file);
  }

  function resetBackground() {
    setCustomBg(null);
    localStorage.removeItem(BG_STORAGE_KEY);
  }

  const pageStyle = customBg
    ? { backgroundImage: `url(${customBg})`, backgroundSize: "cover", backgroundPosition: "center", backgroundAttachment: "fixed" }
    : { backgroundColor: DEFAULT_BG_COLOR };

  return (
    <div className="page" style={pageStyle}>
      <header className="header">
        <img src="/cdu-logo.png" alt="Charles Darwin University" className="logo" />
      </header>

      <main className="card">
        <h1>Tasks</h1>

        {error && <div className="error">{error}</div>}

        <form onSubmit={addTask} className="add-form">
          <input
            value={title}
            onChange={(e) => setTitle(e.target.value)}
            placeholder="New task title"
          />
          <button type="submit">Add</button>
        </form>

        {loading ? (
          <p>Loading tasks…</p>
        ) : tasks.length === 0 ? (
          <p className="empty">No tasks yet — add one above.</p>
        ) : (
          <ul className="task-list">
            {tasks.map((t) => (
              <li key={t.id} className={t.isDone ? "done" : ""}>
                <label>
                  <input type="checkbox" checked={t.isDone} onChange={() => toggleDone(t)} />
                  <span>{t.title}</span>
                </label>
                <button className="delete-btn" onClick={() => deleteTask(t.id)}>
                  Delete
                </button>
              </li>
            ))}
          </ul>
        )}
      </main>

      <input
        ref={fileInputRef}
        type="file"
        accept="image/*"
        onChange={handleBackgroundUpload}
        style={{ display: "none" }}
      />
      <div className="bg-fab-group">
        <button className="bg-fab" onClick={() => fileInputRef.current?.click()}>
          Background
        </button>
        {customBg && (
          <button className="bg-fab bg-fab-reset" onClick={resetBackground}>
            Reset
          </button>
        )}
      </div>
    </div>
  );
}
