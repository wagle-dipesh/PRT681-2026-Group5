# Week 6 — Five Business Elements & How They Connect

Practice pack for **Customer Order Tracking Enhancement** (My Orders).
Builds on Week 4 BRD / stories and Week 5 UAT, rather than starting a new product.

**Author:** Jianlong Jeremy Chen (s392088)  
**Role:** Business Analyst  
**Sources:** `Week4_Sample_BRD_Template.md`, `Week4_JIRA_Backlog_Practice.md`,
`Week5_UAT_Test_Plan.md`

---

## 1. Why these five elements

A BRD that jumps straight to "the system shall…" hides *why* the work exists.
Week 6 asks the BA to name the layers above and below a user story, then show
that they form one chain:

```
Business objective
    → Capability (what the business must be able to do)
        → Business process (L1 area → L2 process → L3 task)
            → User persona (who does the work / feels the pain)
                → User story (value in the user's words)
                    → Requirement (what the system must do / how well)
```

If any layer is missing, the team still *builds*, but cannot explain *why*,
*for whom*, or *how we know it is done*.

---

## 2. Capabilities

**What they are:** stable business abilities. They outlive a sprint. Screens and
APIs implement a capability; they are not the capability itself.

| ID | Capability | What the business can do | Value (from BRD objectives) |
|---|---|---|---|
| CAP-01 | Order self-service visibility | Let a logged-in customer see their own past orders without calling support | BO-1 Reduce order-status support calls by 50% |
| CAP-02 | Order detail inspection | Let the customer open one order and see date, items, amount and status | BO-1 + BO-2 (fewer "what did I buy?" calls; better CSAT) |
| CAP-03 | Account-scoped order privacy | Restrict order data to the authenticated owner | Protects BO-1 (trust) and NFR-02 |
| CAP-04 | Order-history performance | Keep the list usable as history grows (load time + pagination) | Protects BO-1 at scale (NFR-01) |

**Not a capability:** "My Orders page", "GET /orders API", "React list component".
Those are *solutions* that realise CAP-01.

---

## 3. Business processes (three levels)

OneNote requires all three levels. The BA uses L1 to stay in scope, L2 to name
the process the story sits in, and L3 to write tasks, Gherkin and UAT steps.

| Level | Name | Description | My Orders example |
|---|---|---|---|
| **L1** | Process area | Broad value stream | Post-purchase **Order-to-Cash support** (after payment, before / after dispatch) |
| **L2** | Process | A named flow inside that area | **Track my order status** (self-service) |
| **L3** | Task / step | A single action a person or system performs | Log in → open My Orders → scan list → open detail → leave (or call support if still unclear) |

### L3 task list for "Track my order status"

| Step | Actor | Task | Outcome |
|---|---|---|---|
| 3.1 | Customer | Authenticate | Session bound to one customer id |
| 3.2 | Customer | Open **My Orders** | List request sent |
| 3.3 | System | Load orders for that customer only | Rows or empty result |
| 3.4 | System | Sort by date, newest first; paginate at 20 (Sprint 2) | Usable list |
| 3.5 | Customer | Read date, amount, status | Decide whether to open a row |
| 3.6 | Customer | Open one order | Detail: items + current status |
| 3.7 | Customer | (If still blocked) contact support | Call should now be the exception |

Week 4 BPMN modelled **order fulfilment** (place → pay → pick → dispatch).
That is a *different* L2 process. This week's L2 is **status enquiry**, which
is why a new Activity diagram is needed (see `Week6_Advanced_Diagrams.md`).

---

## 4. User personas

Personas keep stories from being written as "the user wants…". Names below are
practice constructs, aligned to the Week 5 stakeholder map.

### P-01 Priya — Registered customer (primary)

| Field | Detail |
|---|---|
| Role | Repeat online shopper |
| Goal | Confirm "did it go through?" and "what is the status?" in under a minute |
| Pain | Today she emails or calls support (~40 status calls/week across the book) |
| Behaviour | Logs in on mobile after checkout; rarely reads long help articles |
| Quote | "I just need to see my last order. If the page is empty I think the site is broken." |
| Stories she owns | STORY-1, STORY-2, STORY-3, STORY-4 |

### P-02 Marcus — Customer Support Manager

| Field | Detail |
|---|---|
| Role | Owns the support queue |
| Goal | Cut "where is my order?" volume so agents handle exceptions only |
| Pain | Agents re-key order numbers from the back-office tool while the customer waits |
| Behaviour | Will run UAT as a proxy end user (Week 5 plan) |
| Quote | "If they can see Pending vs Completed themselves, we stop reading the same three fields aloud." |
| Interest | UAT-01 / UAT-02 pass; empty-state (STORY-3) so Priya does not raise a "bug" when she has no orders |

### P-03 Elena — Head of E-commerce (business owner)

| Field | Detail |
|---|---|
| Role | Accountable for CSAT and the 50% call-reduction target |
| Goal | Ship a Must slice in Sprint 1; do not expand into courier tracking |
| Pain | Scope creep (cancel-from-detail, live tracking) delaying the self-service page |
| Behaviour | Signs UAT; parks CR-01 (Week 5) as Phase 2 |
| Quote | "Show me the RTM: every Must has a test, and tracking is still out of scope." |

**How to use them:** Priya's quote justifies FR-04 (empty state). Elena's quote
justifies keeping courier tracking **out of scope**. Marcus is the UAT executor,
not the persona the UI is designed *as*.

---

## 5. User stories (value in the user's words)

IDs match the Week 4 backlog. Week 6 does not invent a new epic.

**EPIC-1: Customer Order Self-Service** — allow customers to view order history
and status without contacting support.

| ID | Story | Persona | Capability | Sprint |
|---|---|---|---|---|
| STORY-1 | As a customer, I want to see a list of my past orders, so I can track my purchases. | P-01 Priya | CAP-01 | Sprint 1 (Must) |
| STORY-2 | As a customer, I want to click into an order to see full details, so I can check what I bought. | P-01 Priya | CAP-02 | Sprint 1 (Must) |
| STORY-3 | As a customer, I want a clear message when I have no orders, so the page doesn't look broken. | P-01 Priya | CAP-01 | Sprint 2 (Should) |
| STORY-4 | As a customer, I want the order list paginated, so large histories load quickly. | P-01 Priya | CAP-04 | Sprint 2 (Could) |

Privacy is not a separate story: it is an AC / NFR on STORY-1 (CAP-03, UAT-05).

INVEST check (Sprint 1): STORY-1 and STORY-2 are independently testable, valuable
to Priya, small enough to estimate, and already have Gherkin.

---

## 6. Requirements (system view)

Stories speak as the user. Requirements speak as the system. Same IDs as Week 4.

### Functional

| ID | Requirement | Story | Capability |
|---|---|---|---|
| FR-01 | The system shall display a list of the logged-in customer's past orders. | STORY-1 | CAP-01 |
| FR-02 | The system shall show order date, amount and status for each order. | STORY-1 | CAP-01 |
| FR-03 | The system shall allow the customer to click an order to view full detail. | STORY-2 | CAP-02 |
| FR-04 | The system shall display a friendly message when a customer has no orders. | STORY-3 | CAP-01 |

Statuses in scope remain **Pending, Completed, Cancelled** (BRD). Real-time
courier tracking stays out of scope.

### Non-functional

| ID | Requirement | Story | Capability |
|---|---|---|---|
| NFR-01 | List loads within 2 seconds for up to 100 orders; paginate at 20. | STORY-4 | CAP-04 |
| NFR-02 | Order data visible only to the authenticated owner. | STORY-1 (security AC) | CAP-03 |

---

## 7. How the five elements interconnect (worked example)

Take BO-1: *reduce order-status support calls by 50%*.

1. **Capability** — the business must be able to offer **order self-service
   visibility** (CAP-01), not only a staff back-office screen.
2. **Process** — that capability is exercised in L2 **Track my order status**,
   specifically L3 steps 3.1–3.5 (login → list). Fulfilment BPMN is upstream;
   it *creates* the order record this process *reads*.
3. **Persona** — Priya is the actor on those L3 steps; Marcus feels the volume
   if she cannot complete them; Elena owns the 50% target.
4. **User story** — STORY-1 is Priya's slice of CAP-01; STORY-2 is CAP-02 when
   the list is not enough.
5. **Requirement** — FR-01 / FR-02 make STORY-1 testable; NFR-02 (CAP-03) stops
   a "successful" list that leaks another customer's orders; UAT-01 / UAT-05
   prove both.

Same chain for the empty-state pain in Priya's quote:

`BO-2 CSAT → CAP-01 → L3 "open My Orders with zero rows" → Priya → STORY-3 → FR-04 → UAT-03`

If STORY-3 slips to Sprint 2, the RTM must show that CAP-01 is only *partially*
realised in Sprint 1 — that is an honest gap, not a missing row.

```
BO-1 / BO-2
    CAP-01 visibility ── STORY-1 ── FR-01, FR-02 ── UAT-01
                      └─ STORY-3 ── FR-04         ── UAT-03 (Sprint 2)
    CAP-02 inspection ── STORY-2 ── FR-03         ── UAT-02
    CAP-03 privacy    ── STORY-1 ── NFR-02        ── UAT-05
    CAP-04 performance── STORY-4 ── NFR-01        ── UAT-04 (Sprint 2)
```

Full row-by-row trace: `Week6_RTM.md`.  
Behaviour of L3 and of order status: `Week6_Advanced_Diagrams.md`.

---

## 8. What to tell Dev, QA and DA in the walkthrough

| Role | What this document is for |
|---|---|
| **Dev** | CAP-01/02 are read-only against the existing Orders table (Week 4 constraint). Do not redesign schema for courier tracking. CAP-03 is a hard filter on customer id, not a UI hide. |
| **QA / Test** | Every Must capability has a UAT case. CAP-04 / STORY-3 are later — do not treat them as Sprint 1 holes. |
| **DA** | L3 "load orders" is a read of `Orders` (+ line items for detail). Privacy means queries are always scoped by authenticated customer id. |

---

## 9. Key Takeaway

Capabilities and L1/L2/L3 processes explain *why a story exists*. Personas
explain *who cares*. Stories and FR/NFRs explain *what ships and how we test it*.
Week 6 is the week those five stay visibly joined — the RTM and diagrams are
the same chain drawn as a table and as pictures, not a separate assignment.
