# Week 4 — Advanced Enterprise UI, Next.js & Modern Dev Workflows

## Course notes

**Next.js Essential Training**
- Next.js's **App Router** organises routes by folder structure under `app/` — `app/page.tsx` is
  the route at `/`, and each folder can have its own `page.tsx`, `layout.tsx`, etc.
- **Server Components** (the default in the App Router) run on the server and can `fetch` data
  directly — no separate API-calling `useEffect` needed for the initial render. **Client
  Components** (marked `"use client"`) run in the browser and are needed for anything interactive
  (state, event handlers, browser APIs).
- **SSR vs SSG**: SSR (Server-Side Rendering) generates the HTML fresh on every request (used
  here, since the task list changes constantly); SSG (Static Site Generation) generates HTML once
  at build time — not appropriate for live, frequently-changing data like this task list.

**Kendo UI (KendoReact)**
- Commercial component library (Progress Software) with production-grade components (Grid,
  Scheduler, Charts, pivot tables) that would take significant effort to build from scratch —
  particularly the editable Grid's inline-edit, validation, and virtualization behaviour.
- Used the free trial license for this practice exercise, as noted in the README.

**AI-Assisted Engineering ("vibe coding")**
- Used Claude (Anthropic's AI assistant) as the prompt-driven coding tool for this exercise —
  scaffolding the Next.js project structure, the KendoReact Grid boilerplate, and the API-calling
  logic, then reviewing and adjusting the generated code rather than writing every line by hand.
  Flagging this explicitly rather than the task's example tools (Copilot/Cursor), since that's
  what was actually used here — the underlying practice (prompt-driven scaffolding, then
  human review) is the same skill regardless of which specific tool.
- The main judgement calls that still needed human review after AI scaffolding: choosing
  optimistic vs. pessimistic update patterns for the grid edits, and deciding where the
  SSR/client-component boundary should sit (`page.tsx` vs `TasksGrid.tsx`).

## Key concepts
- **SSR vs. client-side hydration**: the server sends fully-rendered HTML (so the page has
  content immediately, good for perceived speed and SEO), then React "hydrates" that HTML in the
  browser — attaching event handlers and making it interactive. This project demonstrates
  both halves: `page.tsx` (server-rendered initial data) → `TasksGrid.tsx` (hydrated, interactive).
- **Commercial enterprise component libraries**: the appeal isn't just visual polish — it's
  accessibility, keyboard navigation, virtualization for large datasets, and consistent behaviour
  that would take a real team weeks to build and test from scratch for something like an editable
  data grid.

## Practice deliverable
See `tasks-portal/` — a Next.js (App Router) management portal using KendoReact's Grid component,
reusing the same Tasks API from Week 5/6. Server-renders the initial task list, then hydrates into
an editable grid (inline edit, add, delete) that calls the ASP.NET Core API directly from the
browser.

## Known limitations
- No authentication on the portal or the API — consistent with every prior version of this
  exercise, fine for local practice.
- KendoReact's free trial license shows a watermark/banner in the UI until a real license key is
  configured — expected for practice use, not something to "fix" without an actual paid license.
