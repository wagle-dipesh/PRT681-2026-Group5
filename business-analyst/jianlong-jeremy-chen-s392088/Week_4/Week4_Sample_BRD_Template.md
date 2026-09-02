# Sample Business Requirements Document (BRD) Template
### (Self-created practice exercise, Week 4)

## 1. Document Control
| Field | Detail |
|---|---|
| Project Name | Customer Order Tracking Enhancement |
| Author | [Your Name] |
| Version | 0.1 (Draft) |
| Status | In Review |

## 2. Executive Summary
Brief (2–3 sentence) description of the business problem and proposed solution.

> Example: Customers currently have no way to check the status of their order online,
> resulting in ~40 support calls per week asking "where is my order". This project
> proposes adding a self-service "My Orders" page.

## 3. Business Objectives
- Reduce order-status support calls by 50% within 3 months of launch.
- Improve customer satisfaction score (CSAT) related to order tracking.

## 4. Scope

**In Scope:**
- Customer-facing "My Orders" list and detail page.
- Order status shown: Pending, Completed, Cancelled.

**Out of Scope:**
- Real-time courier tracking integration (future phase).
- Order editing/cancellation from this page.

## 5. Stakeholders
| Name/Role | Interest |
|---|---|
| Customer Support Manager | Wants reduced call volume |
| Head of E-commerce | Wants improved customer experience |
| Development Team Lead | Needs clear, testable requirements |

## 6. Functional Requirements
| ID | Requirement | Priority (MoSCoW) |
|---|---|---|
| FR-01 | The system shall display a list of the logged-in customer's past orders. | Must |
| FR-02 | The system shall show order date, amount and status for each order. | Must |
| FR-03 | The system shall allow the customer to click an order to view full detail. | Must |
| FR-04 | The system shall display a friendly message when a customer has no orders. | Should |

## 7. Non-Functional Requirements
| ID | Requirement |
|---|---|
| NFR-01 | The order list page shall load within 2 seconds for up to 100 orders. |
| NFR-02 | Order data shall only be visible to the authenticated owner of the order. |

## 8. Assumptions & Constraints
- Assumes customers already have an authenticated account.
- Constraint: must reuse the existing Orders database table (no schema redesign).

## 9. Acceptance Criteria (link to User Stories)
See `Week4_Requirements_and_Prototyping_Notes.md` for the associated user story and
Given-When-Then acceptance criteria.

## 10. Sign-off
| Role | Name | Date |
|---|---|---|
| Business Owner | | |
| Project Sponsor | | |
