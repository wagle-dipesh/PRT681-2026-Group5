# Week 3 — Stakeholder Management & Delivery

## Jira Backlog Setup Guide

### Step 1: Create Free Jira Board
1. Go to https://www.atlassian.com/software/jira/free
2. Sign up with your email (free for up to 10 users)
3. Create a new Scrum project named "eShopOnWeb Team"
4. Set sprint duration to 2 weeks

### Step 2: Epics

**Epic-001: Customer Shopping Experience**
> Enable customers to browse products, manage cart, and complete purchases seamlessly.

**Epic-002: User Authentication & Profile**
> Provide secure registration, login, and account management for all users.

**Epic-003: Admin Catalog Management**
> Allow administrators to maintain product catalog and monitor orders.

**Epic-004: Order Processing & Fulfillment**
> Handle order lifecycle from placement through delivery confirmation.

---

### Step 3: User Stories (Broken Down from Epics)

| Story ID | Epic | Title | Priority | Story Points | Status |
|----------|------|-------|----------|--------------|--------|
| US-001 | Epic-001 | Browse product catalog | High | 5 | To Do |
| US-002 | Epic-001 | Add items to shopping cart | High | 5 | To Do |
| US-003 | Epic-002 | User registration & login | High | 8 | To Do |
| US-004 | Epic-001 | Place order & checkout | High | 13 | To Do |
| US-005 | Epic-003 | Admin manage product catalog | Medium | 8 | To Do |
| US-006 | Epic-004 | View order history | Medium | 5 | To Do |
| US-007 | Epic-004 | Order status tracking | Low | 5 | To Do |
| US-008 | Epic-002 | Password reset | Medium | 3 | To Do |
| US-009 | Epic-001 | Product search & filter | Medium | 5 | To Do |
| US-010 | Epic-003 | Admin view all orders | Low | 3 | To Do |

---

### Step 4: Priority Rationale (MoSCoW Method)

| Priority | Stories | Reasoning |
|----------|---------|-----------|
| **Must Have** | US-001, US-002, US-003, US-004 | Core e-commerce flow; no purchase possible without these |
| **Should Have** | US-005, US-006, US-008, US-009 | Important for MVP completeness but not blocking |
| **Could Have** | US-007, US-010 | Nice-to-have features for enhanced experience |
| **Won't Have** | Mobile app, Payment gateway, AI recommendations | Out of scope for current sprint |

---

### Step 5: Sprint 1 Planning — Backlog Board Layout

**Sprint 1 Goal:** "Enable a customer to browse products, add to cart, and register/login"

**Sprint 1 Capacity:** 2 weeks, 4 team members, ~30 story points

**Sprint 1 Selected Stories:**
- US-001: Browse product catalog (5 pts)
- US-002: Add items to cart (5 pts)
- US-003: User registration & login (8 pts)
- US-009: Product search & filter (5 pts)
- US-008: Password reset (3 pts)
- **Total: 26 points**

**Sprint 1 Board Columns:**
```
[Backlog] → [To Do] → [In Progress] → [In Review] → [Done]
```

---

## 15-Minute Mock Sprint Planning Script

**Role:** You are the Product Owner / BA facilitating Sprint Planning  
**Audience:** Your development team (or record this for your assignment)  
**Duration:** 15 minutes

---

### **Minute 0–2: Opening & Sprint Goal**
> "Good morning team. Welcome to Sprint 1 Planning. Our goal for this sprint is: **Enable a customer to browse products, add to cart, and register an account.** This is our foundation — without these, nothing else works. Any questions on the goal before we dive in?"

---

### **Minute 2–5: Review Backlog & Priorities**
> "Looking at our prioritized backlog, I've selected these stories for Sprint 1 based on our 30-point capacity and dependencies:
>
> 1. **US-001 — Browse Catalog (5 pts):** This is our entry point. Customer lands on homepage and sees products.
> 2. **US-002 — Add to Cart (5 pts):** Builds on US-001. Customer needs to collect items before buying.
> 3. **US-003 — Registration & Login (8 pts):** Required before checkout. Biggest story, so let's discuss complexity.
> 4. **US-009 — Search & Filter (5 pts):** Enhances catalog browsing. Medium priority but high user value.
> 5. **US-008 — Password Reset (3 pts):** Small story, good filler if we finish early.
>
> Total: 26 points. Does this feel realistic for 2 weeks?"

---

### **Minute 5–8: Story Discussion & Clarification**
> "Let's walk through the highest-priority stories. Dev team — any technical concerns?
>
> **US-003 — Registration & Login (8 pts):** This involves ASP.NET Identity integration, password hashing, email validation. Is 8 points accurate, or should we split this into 'Registration' and 'Login' as separate stories?
>
> *[Wait for team response]*
>
> **US-002 — Add to Cart (5 pts):** We need to decide — session-based cart for guests, or database cart only for logged users? eShopOnWeb uses a hybrid approach. What's our target for this sprint?
>
> *[Wait for team response]*
>
> Any other clarifications needed before we commit?"

---

### **Minute 8–11: Task Breakdown & Assignment**
> "Let's break US-001 into tasks:
>
> - **Task 1.1:** Create Product entity and database migration — assigned to Dev Lead
> - **Task 1.2:** Build product listing page UI — assigned to Frontend Dev
> - **Task 1.3:** Implement pagination logic — assigned to Backend Dev
> - **Task 1.4:** Write unit tests for product service — assigned to Tester
>
> US-002 tasks:
> - **Task 2.1:** Create CartItem entity and relationships
> - **Task 2.2:** Implement add-to-cart API endpoint
> - **Task 2.3:** Update header cart icon with item count
> - **Task 2.4:** Write integration tests for cart operations
>
> Does everyone have clear tasks for the first few days?"

---

### **Minute 11–13: Definition of Done (DoD) Review**
> "Before we commit, let's confirm our Definition of Done for every story this sprint:
>
> 1. Code is written and peer-reviewed
> 2. Unit tests pass (minimum 80% coverage)
> 3. Acceptance criteria are met and verified by Tester
> 4. BA reviews and approves the feature against user story
> 5. Documentation is updated (if applicable)
> 6. No critical or high bugs open
>
> Everyone agrees this is our standard?"

---

### **Minute 13–15: Commitment & Close**
> "Great. Team — do we commit to these 26 points for Sprint 1? Raise your hand if yes.
>
> *[Team votes]*
>
> Committed. Our Sprint 1 backlog is locked. Daily standups at 9:00 AM. Next review in 2 weeks. Any blockers, flag them immediately in our Teams channel. Let's build this. Meeting adjourned."

---

## Stakeholder Management Notes

### Stakeholder Communication Plan

| Stakeholder | Communication | Frequency | Channel | Owner |
|-------------|--------------|-----------|---------|-------|
| Customer | Usability feedback | Per sprint | Survey / Test | BA |
| Admin | Feature updates | Weekly | Email / Demo | BA |
| Business Owner | Sprint review | Bi-weekly | Presentation | BA |
| Developer | Technical specs | Daily | Standup / Jira | Tech Lead |
| Tester | Acceptance criteria | Per story | Jira comments | BA |

### Managing Expectations — Key Principles
1. **Under-promise, over-deliver:** Set realistic sprint goals
2. **Transparent progress:** Jira board is always up to date
3. **Early blocker escalation:** Don't wait until deadline to raise issues
4. **Regular demos:** Show working software every 2 weeks
5. **Document decisions:** Every scope change is recorded and approved

---

## Product Backlog Management Tips

1. **Refinement:** Spend 1 hour mid-sprint grooming next sprint's backlog
2. **Estimation:** Use Planning Poker for story points (Fibonacci: 1, 2, 3, 5, 8, 13)
3. **Dependencies:** Map story dependencies before sprint planning
4. **Velocity:** Track completed points per sprint to improve future estimates
5. **Stakeholder feedback:** Add new ideas to backlog, prioritize in next refinement
