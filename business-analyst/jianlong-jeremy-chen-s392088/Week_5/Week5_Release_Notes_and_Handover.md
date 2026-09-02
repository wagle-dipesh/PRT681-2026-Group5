# Week 5 — Release Notes, User Guide & Handover

Practice close-out pack for Sprint 1 of **Customer Order Tracking Enhancement**.
Written as if STORY-1 and STORY-2 had passed UAT (UAT-01, UAT-02, UAT-05).

## 1. Release notes (internal + support-facing)

**Release:** My Orders — Sprint 1
**Audience:** Customer Support, E-commerce, Development
**Date:** [Sprint 1 release date]

### What shipped
- Logged-in customers can open **My Orders** and see their own past orders
  (order number, date, amount, status), newest first.
- Customers can open an order to see line items and current status.
- Orders are not visible across accounts (NFR-02).

### What did not ship (set support expectations)
- Empty-state message for customers with no orders (STORY-3, Sprint 2).
- Pagination for very large histories (STORY-4, Sprint 2).
- Real-time courier tracking (out of scope).
- Cancel / edit order from this page (out of scope; see CR-01 parked).

### Known issues
None open at High/Critical. Low cosmetic items may remain in JIRA.

### Support impact
Expect fewer "where is my order?" calls. If a customer cannot see an order:
1. Confirm they are logged into the same account used at checkout.
2. Confirm the order is not older than the data retention window (assume all
   history for Sprint 1).
3. If still missing, log a defect with customer id + order number — do not
   raise a new feature request.

## 2. Short user guide (customer-facing tone)

### View your orders
1. Sign in.
2. Choose **My Orders**.
3. Orders appear with date, amount and status (Pending, Completed, Cancelled).
4. Select an order to see the items you bought.

### If you have no orders
Sprint 1 may show an empty list. A clearer "You have no orders yet" message
is planned for the next release.

### Privacy
You will only see your own orders. You cannot open someone else's order link.

## 3. Project handover checklist (BA → next sprint / support)

| Item | Location | Owner after handover |
|---|---|---|
| BRD v0.1 | `Week_4/Week4_Sample_BRD_Template.md` | BA (update if CR accepted) |
| Backlog & DoD | `Week_4/Week4_JIRA_Backlog_Practice.md` | Product Owner + Dev Lead |
| RTM | `Week_5/Week5_Traceability_Matrix.md` | BA |
| UAT pack + sign-off | `Week_5/Week5_UAT_Test_Plan.md` | Support Manager (retest on hotfix) |
| Open change requests | CR-01 cancel-from-detail (parked Phase 2) | Product Owner |
| Staging test users | Listed in UAT plan | QA |
| This release note | this file | Support Manager |

**Handover meeting (30 min, practice agenda):**
1. Demo Must journeys (list + detail) — 10 min.
2. Walk RTM and residual risk (empty state, pagination) — 10 min.
3. Confirm support script and parked CR-01 — 10 min.

## 4. Traceability reminder (one slide worth of content)

```
Business goal (fewer status calls)
    → FR-01 / FR-02 / FR-03 / NFR-02
        → STORY-1 / STORY-2
            → UAT-01 / UAT-02 / UAT-05
                → Release notes + support script
```

If any layer is updated, the BA updates the others the same day.

## 5. Key Takeaway

Handover is part of analysis, not an admin afterthought. Support and the next
sprint only inherit what is written down: scope, known gaps, tests that already
passed, and change requests that were deliberately deferred.
