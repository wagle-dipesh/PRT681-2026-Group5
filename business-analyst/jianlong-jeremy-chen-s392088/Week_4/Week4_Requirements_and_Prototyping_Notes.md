# Week 4 — Advanced Requirements Gathering & Prototyping

## 1. Advanced Requirements Gathering Techniques

| Technique | When to use | Notes |
|---|---|---|
| User interviews | Deep-dive into one stakeholder's needs | Best for complex or sensitive requirements |
| Workshops (JAD) | Multiple stakeholders, need consensus | Time-boxed, facilitated, produces shared understanding fast |
| Surveys | Large user base, quantitative input | Good for validating priorities, not for discovering new needs |
| Document analysis | Understanding existing processes/systems | Useful before interviews so questions are more targeted |
| Prototyping (see below) | Requirements are unclear or hard to describe in words | Shows, rather than tells, stakeholders what the solution could look like |

**Key principle:** combine techniques. Interviews uncover the "why", workshops build
agreement, and prototypes validate the "what" before development starts.

## 2. Prototyping & Wireframing Concepts

- A **wireframe** is a low-fidelity, mostly black-and-white layout of a screen — it shows
  structure and content placement, not visual design.
- A **prototype** can be low-fidelity (paper/wireframe) or high-fidelity (clickable mockup)
  and is used to validate requirements with stakeholders before development.
- Benefits: reduces rework, surfaces missing requirements early, gives stakeholders something
  concrete to react to (people find it easier to critique a screen than describe one from
  scratch).

## 3. Sample User Story (created as a practice exercise)

```
Title: Customer views order history

As a registered customer
I want to view a list of my past orders
So that I can check the status and details of previous purchases

Acceptance Criteria (Given-When-Then):
1. Given I am logged in, when I navigate to "My Orders", then I see a list of all my
   orders sorted by date (most recent first).
2. Given I have no past orders, when I navigate to "My Orders", then I see a message
   "You have no orders yet."
3. Given I select an order from the list, when the order detail page loads, then I see
   the order date, items, amount and current status.
```

## 4. Sample Low-Fidelity Wireframe (text-based sketch)

Created as a quick practice exercise before using Balsamiq.

```
--------------------------------------------------
| LOGO           Search...          [My Account]  |
--------------------------------------------------
| MY ORDERS                                        |
|--------------------------------------------------|
| Order #101   | 01 Jul 2026 | $250.00 | Completed |
| Order #104   | 04 Jul 2026 | $300.00 | Pending    |
| Order #109   | 10 Jul 2026 | $200.00 | Pending    |
--------------------------------------------------
|              [ View Order Details ]              |
--------------------------------------------------
```

## 5. Tools Explored

| Tool | Purpose |
|---|---|
| Balsamiq | Low-fidelity wireframing |
| JIRA / Trello | Logging and tracking user stories / backlog items |

## 6. Key Takeaways
- Advanced elicitation is about triangulating multiple techniques, not relying on one.
- A wireframe doesn't need to be polished — its job is to remove ambiguity fast.
- Every user story should have clear, testable acceptance criteria before it goes into
  a sprint backlog.
