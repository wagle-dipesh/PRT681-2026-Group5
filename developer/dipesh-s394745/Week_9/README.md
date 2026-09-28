# Resilient Orders API (Week 5 Practice)

A small ASP.NET Core Web API that demonstrates the Week 5 topics: structured logging, exception tracking, durable workflow orchestration, async email dispatch, and Docker on a Linux (Alpine) base image.

**The scenario:** a customer places an order (`POST /api/orders`). Instead of sending the confirmation email inline and making the customer wait (and losing the email entirely if the SMTP server is briefly unreachable), the API hands the job to a **Temporal workflow** that runs in the background, retries automatically on failure, and survives the API process restarting.

## What's inside

| Requirement | How it's done |
|---|---|
| Structured logging (Serilog/Seq) | Serilog logs to the console **and** to [Seq](https://datalust.co/seq), with named properties like `OrderId`, not just plain text |
| Exception tracking (ELMAH) | [ElmahCore](https://github.com/ElmahCore/ElmahCore) records every unhandled exception with its full stack trace, viewable at `/elmah` |
| Durable orchestration (Temporal.io) | An `OrderConfirmationWorkflow` (+ `EmailActivities` activity) runs on a Temporal worker hosted inside the API, with an automatic retry policy |
| Async email (MailKit/SMTP) | The workflow's activity sends a real SMTP email via MailKit to a local dev mail catcher (**smtp4dev**), so no real email account or credentials are needed |
| Linux container (Alpine) | `Dockerfile` builds/runs on `mcr.microsoft.com/dotnet/aspnet:8.0-alpine`, with a `HEALTHCHECK` hitting `/health` |

## How to run it (one command)

You need [Docker Desktop](https://www.docker.com/products/docker-desktop/) running.

```bash
docker compose up -d --build
```

This starts **4 containers**:

| Service | URL | What it's for |
|---|---|---|
| `api` | http://localhost:8090 | the Web API itself (Swagger UI at `/swagger`) |
| `seq` | http://localhost:5341 | browse the structured logs |
| `temporal` | http://localhost:8233 | Temporal's Web UI - see the workflow run |
| `smtp4dev` | http://localhost:5000 | see the "sent" confirmation emails |

Check everything came up healthy:

```bash
docker compose ps
```

The `api` row should say `(healthy)` - that's the Docker `HEALTHCHECK` we added actually working, not just "container is running."

## Try it

**1. Create an order:**

```bash
curl -X POST http://localhost:8090/api/orders \
  -H "Content-Type: application/json" \
  -d "{\"customerName\":\"Alice Nguyen\",\"customerEmail\":\"alice@example.com\",\"product\":\"Wireless Mouse\",\"quantity\":2}"
```

You get back the order immediately (`202 Accepted`) plus a `workflowId` - the API didn't wait for the email to actually send.

**2. Check the order a moment later** (swap `1` for the id you got back):

```bash
curl http://localhost:8090/api/orders/1
```

`"emailConfirmed": true` means the Temporal workflow's activity successfully sent the email.

**3. See the email** at http://localhost:5000 (smtp4dev's inbox) — a real SMTP email, just caught locally instead of leaving your machine.

**4. See the workflow run** at http://localhost:8233 — search for `order-confirmation-1`. You can see the workflow's history: it started, called the `SendOrderConfirmationEmailAsync` activity, and completed.

**5. See the structured logs** at http://localhost:5341 — every log line is queryable by property, e.g. type `OrderId == 1` in the search box.

**6. Trigger the exception-tracking demo:**

```bash
curl http://localhost:8090/throw
```

Then look at http://localhost:8090/elmah — you'll see the full exception with its stack trace, captured automatically with no try/catch written for it.

## How the resiliency actually shows up

Stop the mail catcher while the API is still handling an order, and Temporal will retry the email-sending activity instead of losing the order:

```bash
docker compose stop smtp4dev
curl -X POST http://localhost:8090/api/orders -H "Content-Type: application/json" -d "{\"customerName\":\"Bob\",\"customerEmail\":\"bob@example.com\",\"product\":\"Keyboard\",\"quantity\":1}"
# check the Temporal UI (localhost:8233) for this workflow - you'll see it retrying the activity
docker compose start smtp4dev
# within a few seconds, the retry succeeds and the email shows up in smtp4dev's inbox
```

That's the "durable execution against network drops" concept from this week's topics: the workflow doesn't just fail and drop the order - Temporal remembers it was in progress and keeps retrying until it succeeds (up to the 5 attempts configured in `Workflows/OrderConfirmationWorkflow.cs`).

## Code tour

- [`Program.cs`](Week9_ResilientOrdersApi/Program.cs) — wires up Serilog, ElmahCore, health checks, and the `/api/orders` + `/throw` endpoints.
- [`Models/Order.cs`](Week9_ResilientOrdersApi/Models/Order.cs) — the `Order` model and a simple in-memory `OrderStore` (no database, same approach as previous weeks' practice).
- [`Workflows/OrderConfirmationWorkflow.cs`](Week9_ResilientOrdersApi/Workflows/OrderConfirmationWorkflow.cs) — the Temporal workflow: calls the email activity with a retry policy.
- [`Activities/EmailActivities.cs`](Week9_ResilientOrdersApi/Activities/EmailActivities.cs) — the Temporal activity that actually sends the email via MailKit.
- [`Workflows/TemporalWorkerService.cs`](Week9_ResilientOrdersApi/Workflows/TemporalWorkerService.cs) — a `BackgroundService` that connects to the Temporal server and runs the worker for the lifetime of the API.
- [`Dockerfile`](Week9_ResilientOrdersApi/Dockerfile) — multi-stage build: `sdk:8.0-alpine` to build, `aspnet:8.0-alpine` to run, with `curl` added just for the `HEALTHCHECK`.
- [`docker-compose.yml`](docker-compose.yml) — the API plus its three supporting services (Seq, Temporal, smtp4dev).

## Running it without Docker (optional)

If you'd rather run the API with `dotnet run` on your machine while the three supporting services still run in Docker:

```bash
docker compose up -d seq temporal smtp4dev
cd Week9_ResilientOrdersApi
dotnet run
```

`appsettings.json` already points at `localhost` for all three, so this works without changing anything.

## Notes

- Orders are stored in memory only (same pattern as the Week 4 practice) - restarting the API loses the order list, but **not** any Temporal workflow that's already in progress (Temporal keeps its own durable history independently of the API process).
- Seq is started with `SEQ_FIRSTRUN_NOAUTHENTICATION=true` - fine for a local demo, but a real deployment would set an admin password instead.
- smtp4dev and Temporal's dev server are both meant for local development/testing, not production - a real system would use a real SMTP provider and a production Temporal Cluster (or Temporal Cloud).
