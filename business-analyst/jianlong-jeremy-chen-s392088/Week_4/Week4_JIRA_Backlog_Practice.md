# Week 4 — JIRA Backlog Practice Notes

Practice exercise: created a sample backlog for the "My Orders" feature in a personal
JIRA project, to practise translating BRD requirements into trackable backlog items.

## Epic
**EPIC-1: Customer Order Self-Service**
> Allow customers to view their order history and status without contacting support.

## User Stories Created

| ID | Story | Priority | Story Points |
|---|---|---|---|
| STORY-1 | As a customer, I want to see a list of my past orders, so I can track my purchases. | Must | 3 |
| STORY-2 | As a customer, I want to click into an order to see full details, so I can check what I bought. | Must | 2 |
| STORY-3 | As a customer, I want a clear message when I have no orders, so the page doesn't look broken. | Should | 1 |
| STORY-4 | As a customer, I want the order list paginated, so large histories load quickly. | Could | 3 |

## Sprint Planning Practice
- Sprint length used: 2 weeks
- Sprint 1 goal: STORY-1 and STORY-2 (core "must have" functionality)
- Sprint 2 goal: STORY-3 and STORY-4 (polish + performance)

## Definition of Done (practised writing one)
A story is "Done" when:
1. Code is merged and passes automated tests.
2. Acceptance criteria (Gherkin scenarios) all pass.
3. Product Owner has reviewed and accepted the feature in a staging environment.
4. Documentation/BRD is updated to reflect any changes made during development.

## Key Takeaway
Breaking the BRD's functional requirements (FR-01 to FR-04) into individual, small,
independently testable stories made estimation and sprint planning much easier than
trying to plan against the BRD directly.
