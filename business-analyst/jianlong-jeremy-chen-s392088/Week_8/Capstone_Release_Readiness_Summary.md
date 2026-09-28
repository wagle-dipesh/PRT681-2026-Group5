# 04 - Capstone Release Readiness Package & Presentation Guide

**Module:** PRT681 / PRT585 - Business Analyst (BA)  
**Deliverable Type:** Unified Release Readiness Portfolio (BRD + BPMN + Figma + RTM) for 15-Minute Team Walkthrough & Git Sync

---

## 1. Release Readiness Artifacts Checklist

1. **Business Requirements Document (BRD Final Draft)**
   * Explicit scope boundaries: Clear separation of In-Scope (one-click reorder, stock check, 60s cancellation) vs. Out-of-Scope (cross-restaurant multi-orders, custom item modifications).
2. **Business Process Models (BPMN 2.0 Swimlanes)**
   * As-Is Flow: Highlights manual menu searching and cart abandonment bottlenecks.
   * To-Be Flow: Demonstrates automated gateways for real-time inventory checks and timer-based cancellation.
3. **Figma 4-Screen Interactive Prototype**
   * Validated click-through sequence: `Order_History` $\rightarrow$ `Item_Availability` $\rightarrow$ `Cart_Checkout` $\rightarrow$ `Confirmation_GracePeriod`.
4. **End-to-End Requirements Traceability Matrix (RTM)**
   * Provides bi-directional mapping from business goals to automated/manual UAT test cases.

---

## 2. 15-Minute Team Walkthrough Presentation Agenda

* **[00:00 - 03:00] Business Context & Value Definition**
  * Present strategic goals: Lowering reorder friction to increase customer retention and order volume.
  * Define scope boundaries locked in for this release increment.
* **[03:00 - 08:00] Process Architecture & Figma Prototype Demo**
  * Walk through the BPMN decision gateway logic alongside the 4-screen interactive wireframe.
  * Highlight edge-case handling: UI and cart behavior when handling sold-out dishes.
* **[08:00 - 12:00] Acceptance Criteria (Gherkin) & RTM Traceability**
  * Review BDD Given-When-Then scenarios with Dev and QA leads to verify contract alignment.
  * Demonstrate 100% test coverage across all stories using the RTM grid.
* **[12:00 - 15:00] Q&A, Feedback & DoD Sign-off**
  * Address technical feedback regarding the 60-second timer implementation; confirm Definition of Done (DoD).

---

## 3. Weekly Timesheet Log Entries (6.0 Hours Total)

* **Hour 1:** Studied enterprise NFR standards and defined latency, concurrency, and security SLA criteria.
* **Hour 2:** Authored end-to-end Gherkin (Given-When-Then) acceptance criteria for the entire reorder flow.
* **Hour 3:** Built an end-to-end RTM in Excel linking business goals, user stories, wireframes, and test cases.
* **Hour 4:** Packaged BRD, BPMN models, Figma wireframes, and RTM into a unified release readiness portfolio.
* **Hour 5:** Updated single-page resume with Week 4-5 academic practice projects and verified interview Q&A.
* **Hour 6:** Conducted team end-to-end walkthrough presentation, gathered feedback, and synced artifacts to Git.