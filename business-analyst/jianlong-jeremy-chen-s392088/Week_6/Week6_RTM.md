# Week 6 — Requirements Traceability Matrix (RTM)

Extended RTM for **Customer Order Tracking Enhancement**.

Week 5 traced:

`Business objective → BRD requirement → User story → Gherkin / AC → UAT case`

Week 6 inserts **Capability** so the matrix matches the five-element chain in
`Week6_Business_Elements.md`:

`Business objective → Capability → User story → Test case`

Requirement IDs are kept so this file can sit beside
`Week_5/Week5_Traceability_Matrix.md` without renaming FR / NFR / UAT rows.

**Author:** Jianlong Jeremy Chen (s392088)

---

## 1. What changed from Week 5

| Week 5 | Week 6 |
|---|---|
| Objective → requirement → story → test | Objective → **capability** → story → test |
| Answered "is this FR tested?" | Also answers "does this story realise a named business ability?" |
| FR-04 / NFR-01 shown as later | Same, but now visible as **partial CAP-01 / deferred CAP-04** |

Forward trace: *this objective is delivered by these capabilities, stories and tests.*  
Backward trace: *this UAT fail maps to a story, a capability, and a stakeholder.*

---

## 2. ID legend (unchanged from Weeks 4–5)

| Prefix | Meaning | Source |
|---|---|---|
| BO-1, BO-2 | Business objectives | Week 4 BRD |
| CAP-01 … CAP-04 | Capabilities | `Week6_Business_Elements.md` |
| FR- / NFR- | Functional / non-functional requirements | Week 4 BRD |
| STORY- | Backlog items under EPIC-1 | Week 4 JIRA practice |
| UAT- | Business-executable cases | Week 5 UAT plan |

---

## 3. Traceability matrix

| Bus. Obj. | Capability | Req. ID | Requirement (short) | Story | UAT case | Priority | Status |
|---|---|---|---|---|---|---|---|
| BO-1 Reduce order-status support calls by 50% | CAP-01 Order self-service visibility | FR-01 | Show the logged-in customer's past orders | STORY-1 | UAT-01 | Must | Ready for UAT (Sprint 1) |
| BO-1 | CAP-01 | FR-02 | Show date, amount and status per order | STORY-1 | UAT-01 | Must | Ready for UAT (Sprint 1) |
| BO-1 | CAP-02 Order detail inspection | FR-03 | Click an order to view full detail | STORY-2 | UAT-02 | Must | Ready for UAT (Sprint 1) |
| BO-2 Improve CSAT for order tracking | CAP-01 | FR-04 | Friendly empty state when there are no orders | STORY-3 | UAT-03 | Should | Sprint 2 |
| BO-1 / performance | CAP-04 Order-history performance | NFR-01 | List loads within 2s for ≤100 orders; paginate at 20 | STORY-4 | UAT-04 | Could | Sprint 2 |
| BO-1 / privacy | CAP-03 Account-scoped order privacy | NFR-02 | Order data visible only to the authenticated owner | STORY-1 (security AC) | UAT-05 | Must | Ready for UAT (Sprint 1) |

Gherkin scenarios that sit behind the UAT column (Week 4):

| UAT | Gherkin scenario |
|---|---|
| UAT-01 | Customer views their order history |
| UAT-02 | Given I select an order… Then I see date, items, amount, status |
| UAT-03 | Customer with no orders sees an empty state |
| UAT-04 | Order list handles a large number of orders |
| UAT-05 | Customer cannot view another customer's orders |

---

## 4. Coverage check

### Sprint 1 (Must) — required for sign-off

| Capability | Realised by | Tests | Gap? |
|---|---|---|---|
| CAP-01 (list) | STORY-1, FR-01, FR-02 | UAT-01 | No — list + columns covered |
| CAP-02 | STORY-2, FR-03 | UAT-02 | No |
| CAP-03 | STORY-1 security AC, NFR-02 | UAT-05 | No |

### Sprint 2 (Should / Could) — not Sprint 1 holes

| Capability | Realised by | Tests | Notes |
|---|---|---|---|
| CAP-01 (empty list) | STORY-3, FR-04 | UAT-03 | Partial CAP-01 until Sprint 2; Priya may still think an empty table is a defect |
| CAP-04 | STORY-4, NFR-01 | UAT-04 | Not in Sprint 1; flag if staging data already exceeds ~20 rows |

**Orphan check**

- No Must requirement without a UAT case.
- No UAT case without a capability and a story.
- Out of scope (courier tracking, cancel-from-detail / CR-01) has **no** row — that is
  deliberate. If a tester invents those cases, they are not tracing this BRD.

---

## 5. How each role uses this matrix

| Role | Use |
|---|---|
| **BA** | Grooming: a new story that cannot sit in a capability row is out of scope or needs a BRD update. Change impact: dropping FR-03 takes STORY-2 and UAT-02 with it. |
| **Dev** | Sprint 1 slice is CAP-01 list + CAP-02 detail + CAP-03 filter. Do not build CAP-04 pagination unless STORY-4 is pulled in. |
| **QA / Test** | Generate the pack from the UAT column. Failed UAT-05 is a CAP-03 / NFR-02 fail — block sign-off. |
| **DA** | CAP-03 implies every list/detail query is keyed by authenticated customer id. CAP-04 implies indexed `OrderDate` / paging, not a full table dump. |
| **Business owner (Elena)** | Signs a version of this table with Pass/Fail, not a verbal "looks good". |

---

## 6. After UAT (same practice results as Week 5)

| UAT case | Result | Defect | RTM action |
|---|---|---|---|
| UAT-01 | Pass | — | Status → Passed; CAP-01 list slice accepted |
| UAT-02 | Pass | — | Status → Passed; CAP-02 accepted |
| UAT-05 | Fail | BUG-12 session timeout still shows previous orders | Status → Failed; CAP-03 not accepted; block sign-off until High defect is fixed and retested |
| UAT-03 / UAT-04 | Not run | — | Remain Sprint 2; do not mark CAP-01 complete or CAP-04 started |

Until UAT-05 is Pass, **CAP-03 is not done**, even if the happy-path list looks correct.

---

## 7. End-to-end chain (one slide)

```
BO-1 fewer status calls / BO-2 CSAT
    CAP-01 visibility  → STORY-1 / STORY-3 → FR-01, FR-02, FR-04 → UAT-01, UAT-03
    CAP-02 inspection  → STORY-2           → FR-03              → UAT-02
    CAP-03 privacy     → STORY-1 (AC)      → NFR-02             → UAT-05
    CAP-04 performance → STORY-4           → NFR-01             → UAT-04
```

Personas and L1–L3 process names live in `Week6_Business_Elements.md`.
Activity / state / data movement for the same chain: `Week6_Advanced_Diagrams.md`.

---

## 8. Key Takeaway

Adding Capability to the RTM stops the team treating every ticket as equal.
STORY-3 and STORY-4 are not "extra UI"; they finish CAP-01 and deliver CAP-04.
A failed UAT-05 is not a cosmetic bug — it means the privacy capability was
never accepted, so Sprint 1 cannot close.
