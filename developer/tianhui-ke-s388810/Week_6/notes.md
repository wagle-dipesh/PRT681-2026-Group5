# Week 6 — Full Stack Integration & Deployment

## Course notes
**Learning Docker**
- Containerised the Week 5 TasksApi (multi-stage build).
- New this time: the containerised API needs to reach a real SQL Server running in a separate
  container on the same machine. Docker's `host.docker.internal` DNS name solves this.

**Azure Fundamentals (AZ-900 prep)**
- Same core concepts as the Week 3 individual-task version — not repeated here.

## Design notes
- Connection string differs inside vs outside the container: `localhost` becomes
  `host.docker.internal` when the API itself is containerised.
- Passed in via environment variable at `docker run` time, not hardcoded.
