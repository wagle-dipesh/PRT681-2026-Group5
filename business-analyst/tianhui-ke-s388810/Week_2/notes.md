# Week 2 — Process Modeling & Tools (Business Analyst)

## Course notes
**Learning BPMN (Business Process Model and Notation)**
- BPMN is a standard notation (events, activities, gateways, flows) for modelling end-to-end business processes so both business and technical people read the same diagram.
- Standard shapes: circle = event (start/end), rounded rectangle = task/activity, diamond = gateway (decision/split/merge), swimlanes = who does what.

**Learning Visio**
- Used to draw BPMN, swimlane, and other process diagrams with reusable stencils and connectors that snap to shapes.
- Diagrams can be exported to PDF/Word and embedded directly into a BRD or Confluence page for stakeholder review.

## Key concepts — capabilities, business processes, personas, stories, requirements
These are interconnected, most clearly explained through this week's Amazon example. A **capability** is *what* the business can do (e.g. "let a customer repurchase something they've bought before"). A **business process** is *how* that capability is carried out step by step (e.g. the actual Buy It Again workflow: check eligibility → check seller/stock → add to cart → confirm → fulfil). A **user persona** represents *who* triggers or is affected by the process (e.g. "Marcus, repeat household-goods buyer"). A **user story** captures a specific need of that persona within the process (e.g. "I want to know if the seller changed"). A **requirement** is the precise, testable statement the solution must satisfy (e.g. "the system must display a seller-change notice when the fulfilling seller differs from the original order's seller"). In short: Capability (what) → Process (how) → Persona (who) → Story (their need) → Requirement (what must be built) — each level gets more specific and closer to what developers actually build.

## Diagram types — explained with examples (Amazon "Buy It Again")

- **Business Process Model and Notation (BPMN) Diagram** — maps an end-to-end process with events/tasks/gateways. *Example: customer opens Buy Again → checks item availability/seller → adds to cart → confirms → payment charged → fulfilment notified.*
- **Use Case Diagram** — shows actors and the system functions ("use cases") they can trigger, using stick figures and ovals. *Example: actor "Customer" connects to use cases "Buy Item Again", "View Order History", "Manage Subscribe & Save"; a second actor "Seller" connects to "Update Stock/Listing" which indirectly affects what Buy Again can offer.*
- **Entity-Relationship (ER) Diagram** — models database entities and their relationships/cardinality. *Example: `Customer (1) --- (many) Order (many) --- (many) Product`, with `Product (many) --- (1) Seller` capturing that a listing's seller can change over time.*
- **Data Flow Diagram (DFD)** — shows how data moves between processes, data stores, and external entities (no control flow/decisions, just data movement). *Example: "Customer" → "Buy Again process" → "Orders data store" and "Catalogue/Seller data store" → "Fulfilment process".*
- **Wireframe or Mockup Diagram** — low-fidelity layout of a screen showing structure/placement, not final visual design. *Example: a box layout of the Buy Again list screen, with each row showing product thumbnail, "Buy Again" button, and a small "sold by [Seller]" label.*
- **Swimlane Diagram (Cross-Functional Flowchart)** — a flowchart split into lanes per role/department, showing handoffs. *Example: lanes for Customer / App / Catalogue Service / Seller / Payment in the Buy Again process — this is exactly the Level 2 diagram flagged as a follow-up in the practice file, since the Level 1 BPMN diagram doesn't show who owns each step.*
- **Gantt Chart** — a horizontal bar chart of tasks against a timeline, used for project scheduling and tracking dependencies. *Example: "Requirements gathering" (Week 1) → "Process modelling" (Week 2) → "Backlog & sprint planning" (Week 3) → "Development" (Weeks 4+).*
- **Stakeholder Map** — plots stakeholders by influence vs. interest to decide how much to engage each one. *Example: Category/Retail team (high influence/high interest) vs. an individual third-party seller (lower influence, but high interest since a seller-change bug directly affects their listing's traffic).*
- **State Diagram** — shows the states an entity can be in and what triggers transitions between them. *Example: Order states: `Cart → Placed → Fulfilled → Delivered / Cancelled / Returned` — note "Returned" specifically matters here since only *delivered, non-returned* orders should populate the Buy Again list (Week 1 business rule).*
- **Requirement Traceability Matrix (RTM)** — a table linking each requirement to its source (stakeholder need), the design element that satisfies it, and the test case that verifies it — proves nothing was missed or built without a reason.
- **Gap Analysis Diagram** — compares current state vs. desired future state to identify what's missing. *Example: "Current: customer re-searches the exact product and re-verifies the variant manually" vs. "Future: one-tap repurchase with automatic variant/seller/price checks" — the gap is the entire Buy Again feature.*
- **Activity Diagram** — similar to a flowchart but from UML, showing the workflow of activities including parallel/concurrent actions using forks and joins. *Example: after "Confirm order", "Charge payment" and "Reserve stock at seller" could happen in parallel before "Send fulfilment confirmation" — modelling this as parallel (rather than strictly sequential, as the simplified BPMN diagram shows it) would matter if stock reservation needs to happen before payment fails, to avoid a race condition where two customers buy the last unit simultaneously.

## Backlog addition (as instructed)
Added to backlog: **SQL Essential Training** — BAs must be able to read data directly (not just request reports) to validate assumptions and write precise requirements grounded in real data. This mattered concretely this week: writing the SQL practice queries surfaced that my first-draft "best-selling products" query didn't actually test the BRD's repurchase premise (see practice file reflection) — something I wouldn't have caught without writing the query myself.

## Practice deliverable
See `practice-BPMN-flowchart-SQL.md` for the BPMN flowchart of the Amazon "Buy It Again" process and 5 SQL SELECT queries against a Northwind-style sample database.
