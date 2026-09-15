# Course Notes: Jira Essential Training

## 1. What is Jira

Jira (by Atlassian) is one of the most widely used agile project management tools. It turns "work to be done" into trackable, assignable, and visualizable items (Issues). It supports Scrum, Kanban, or simple task-list workflows.

Free sign-up: https://www.atlassian.com/software/jira/free

## 2. Key Terminology

| Term | Meaning |
|---|---|
| Project | A workspace container for a team/product |
| Issue | The basic unit of work — includes types like Epic / Story / Task / Bug |
| Epic | Large-grained work made up of multiple stories |
| Story | A user story, completable within one sprint |
| Sub-task | A finer breakdown under a story |
| Backlog | All issues not yet assigned to a sprint |
| Sprint | A fixed-length iteration (e.g. 2 weeks) |
| Board | Visualizes issue status flow — either a Scrum board or a Kanban board |
| Story Point | A unit for estimating a story's relative effort (not hours) |

## 3. Team-managed vs Company-managed Projects

- **Team-managed**: suited for small teams, simple setup, flexible field/workflow configuration — recommended starting point for beginners
- **Company-managed**: suited for cross-team, large-organization use requiring unified governance and permissions

> For personal practice or as a beginner, go straight for a **Team-managed project + Scrum template**.

## 4. Basic Steps to Create a Project

1. Log in to Jira → click **Create project**
2. Choose a template: **Scrum** (for teams working in fixed iterations)
3. Choose **Team-managed**
4. Name the project (e.g. "Week 1 Feature Development")
5. Once inside the project, you'll see a default **Backlog** view and a **Board** view

## 5. Creating an Epic

1. Left navigation → **Backlog** → click the **Epic** panel at the top (or **Roadmap**)
2. Click **Create epic**
3. Fill in the epic name (e.g. "User Registration & Login Improvements") and description
4. Optionally give the epic a color label to distinguish it on the board

## 6. Creating Stories Under an Epic

1. In the Backlog view, click **Create issue**
2. Set Issue Type to **Story**
3. Write the title (use the "As a... I want... so that..." format)
4. Link it to the corresponding epic via the **Epic link** field on the right
5. Fill in **Acceptance Criteria** (in the description field or a custom field)
6. Add a **Story Point** estimate

## 7. Prioritizing

- In the Backlog view, stories can be **dragged and reordered** directly — higher up means higher priority
- You can also tag each story with a Label (e.g. Must / Should / Could, mapping to MoSCoW)
- Order the backlog top-down by priority, with the most important items at the top

## 8. Creating and Starting a Sprint

1. In the Backlog view, click **Create sprint**
2. Drag prioritized stories from the Backlog into the Sprint section
3. Set the sprint name, start/end dates, and Sprint Goal
4. Click **Start sprint**
5. Switch to the **Board** view — you'll see To Do / In Progress / Done columns where stories move through status

## 9. Common Views at a Glance

| View | Purpose |
|---|---|
| Backlog | Manage and order work that hasn't started |
| Board (Scrum) | Track issue status within the current sprint |
| Timeline / Roadmap | View epic-level scheduling |
| Reports (e.g. Burndown) | Track sprint progress and team velocity |

## Summary

Core Jira workflow:
```
Create project → Create epic → Break into stories → Prioritize → Drag into sprint → Start sprint → Move status on the board
```

**Recommended videos** (free, shareable, cover the full workflow):
- Jira Full Course Tutorial (full walkthrough of team-managed projects/Scrum boards/Kanban boards) — https://www.youtube.com/watch?v=wfx8MFrffjo
- Jira for Beginners (FREE COURSE) — https://www.youtube.com/watch?v=NDVSMlVYxm8
- Jira - Tutorial for Beginners in 12 Minutes — https://www.youtube.com/watch?v=fiWaMGCMyk8
