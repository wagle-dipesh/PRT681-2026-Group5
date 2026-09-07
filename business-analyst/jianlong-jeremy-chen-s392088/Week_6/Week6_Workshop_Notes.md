# Week 6 — Workshop Walkthrough (15–20 min)

Facilitation pack for a BA-led walkthrough of the My Orders analysis pack.
Audience: **Developer**, **Tester / QA**, **Data Analyst** (and other BAs if present).

**Author:** Jianlong Jeremy Chen (s392088)  
**Feature:** Customer Order Tracking Enhancement  
**Artefacts walked:** `Week6_Business_Elements.md`, `Week6_RTM.md`,
`Week6_Advanced_Diagrams.md`

Fill in Section 5 after the session. Sections 1–4 are the script.

---

## 1. Goal of the session

In 15–20 minutes the group should share one picture of:

1. Why My Orders exists (objective → capability → persona).
2. What Sprint 1 Must actually is (and what is Sprint 2 / out of scope).
3. How activity, status and data flow constrain the build and the tests.

**Success:** Dev can name the customer-id filter; QA can name the three Must UAT
cases; DA can name the stores that are read vs not touched.

---

## 2. Agenda

| Time | Section | What I show | What I ask the room |
|---|---|---|---|
| 0:00–1:00 | Purpose & scope | BO-1 / BO-2; in vs out of scope | Confirm courier tracking and CR-01 stay out |
| 1:00–6:00 | Five elements | Capabilities, L1–L3, Priya / Marcus / Elena, STORY-1…4 | Dev: is "reuse Orders table" still true? |
| 6:00–10:00 | RTM | Capability column + Must vs Sprint 2 rows | QA: any Must without a case? |
| 10:00–16:00 | Three diagrams | Activity → State → DFD L1 | DA: list query always scoped by customer id? |
| 16:00–20:00 | Actions | Capture blockers | One action each role |

If time is tight, skip persona quotes; do **not** skip CAP-03 / UAT-05 or the DFD
ownership check.

---

## 3. Talking points (aligned to the three docs)

### 3.1 Five elements (≈5 min)

- **Capability ≠ screen.** CAP-01 is "customer can see their own orders". The
  My Orders page is one implementation.
- **Three process levels.** L1 is post-purchase support. L2 is *Track my order
  status*. L3 is login → list → detail. Week 4 BPMN is a different L2 (fulfilment).
- **Persona split.** Priya uses the page. Marcus runs UAT. Elena signs off and
  defends scope.
- **Stories already exist.** No new epic this week. STORY-3 / STORY-4 finish
  CAP-01 empty-state and deliver CAP-04; they are not random polish.

**Ask Dev:** any objection to read-only against the existing Orders table?

### 3.2 RTM (≈4 min)

Walk the Must block only, then point at the Sprint 2 rows:

| Must capability | Story | Test they must not skip |
|---|---|---|
| CAP-01 list | STORY-1 | UAT-01 |
| CAP-02 detail | STORY-2 | UAT-02 |
| CAP-03 privacy | STORY-1 AC | UAT-05 |

- FR-04 / NFR-01 have tests **and** a later sprint — coverage is deferred, not missing.
- Failed UAT-05 (BUG-12 timeout leak) means CAP-03 is not accepted; Sprint 1 cannot close.

**Ask QA:** will UAT-05 be run on the same build as UAT-01, with Customer A/B URLs?

### 3.3 Diagrams (≈6 min)

1. **Activity** — exceptions: not logged in, empty list, paging, wrong-owner URL,
   leftover support call. Happy path should end without support.
2. **State** — only Pending / Completed / Cancelled on the UI. This page does not
   transition state. Do not invent Shipped or a map.
3. **DFD L1** — 1.0 auth, 2.0 list, 3.0 detail (ownership **before** lines),
   4.0 empty vs deny as different payloads. No payment or courier store on this diagram.

**Ask DA:** can list and detail be served with `CustomerID = session` only, without
a table scan of all orders?

---

## 4. Role-specific takeaways (say these out loud)

| Role | Please leave with |
|---|---|
| **Dev** | Sprint 1 = list + detail + owner filter. Pagination and empty-state copy are Sprint 2 unless pulled in. Wrong-owner detail returns deny, not an empty order. |
| **Test** | Must pack = UAT-01, UAT-02, UAT-05. Empty and paging are Sprint 2. State values to seed: Pending and Completed; Cancelled optional but must still be owner-scoped. |
| **DA** | Stores in play: Customer accounts (verify), Orders (list/header), Order lines (detail). Fulfilment writes; this feature reads. Indexes on `CustomerID` + `OrderDate` support CAP-04 later. |

---

## 5. Session record (complete after the workshop)

| Field | Notes |
|---|---|
| Date / duration | |
| Attendees (role + name) | |
| Decision: scope still excludes courier tracking and CR-01? | Yes / No — |
| Dev feedback (API, schema, blockers) | |
| QA feedback (coverage gaps, data, UAT-05) | |
| DA feedback (fields, keys, volume) | |

### Actions

| # | Action | Owner | Due |
|---|---|---|---|
| 1 | | | |
| 2 | | | |
| 3 | | | |

### Parking lot (not this sprint)

| Item | Disposition |
|---|---|
| CR-01 cancel from detail | Already Phase 2 (Week 5) |
| Live courier tracking | Out of scope (BRD) |
| Other | |

---

## 6. Suggested one-slide closer

```
BO-1 / BO-2
  CAP-01 visibility → STORY-1 / STORY-3 → UAT-01 / UAT-03
  CAP-02 inspection → STORY-2           → UAT-02
  CAP-03 privacy    → STORY-1 AC        → UAT-05
  CAP-04 performance→ STORY-4           → UAT-04
```

Activity = how Priya moves. State = what status values exist. DFD = what we
read. RTM = how we prove it.

---

## 7. Key Takeaway

The walkthrough is part of the Week 6 deliverable: the diagrams and RTM only
work if Dev, QA and DA will *use* the same IDs. Capture their objections in
Section 5 the same day; do not leave alignment as a hallway conversation.
