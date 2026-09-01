# Week 5 — Requirements Traceability Matrix (RTM)

Practice RTM for **Customer Order Tracking Enhancement**, linking Week 4 artefacts:

Business objective → BRD requirement → User story → Gherkin / AC → UAT case.

IDs match `Week4_Sample_BRD_Template.md`, `Week4_JIRA_Backlog_Practice.md` and
`Week4_BPMN_and_Gherkin_Practice.md`.

## 1. What an RTM is for

Forward trace: *this business goal is implemented by these stories and tested by these cases.*
Backward trace: *this UAT failure maps back to this requirement and this stakeholder.*

If a row has a requirement but no test, coverage is missing.
If a test has no requirement, the team may be testing something never asked for.

## 2. Traceability matrix

| Bus. Obj. | Req. ID | Requirement (short) | Story | Gherkin / AC | UAT Case | Priority | Status |
|---|---|---|---|---|---|---|---|
| BO-1 Reduce order-status support calls by 50% | FR-01 | Show the logged-in customer's past orders | STORY-1 | Scenario: Customer views their order history | UAT-01 | Must | Ready for UAT |
| BO-1 | FR-02 | Show date, amount and status per order | STORY-1 | Then I see date / amount / status on each row | UAT-01 | Must | Ready for UAT |
| BO-1 | FR-03 | Click an order to view full detail | STORY-2 | Given I select an order… Then I see date, items, amount, status | UAT-02 | Must | Ready for UAT |
| BO-2 Improve CSAT for order tracking | FR-04 | Friendly empty state when there are no orders | STORY-3 | Scenario: Customer with no orders sees an empty state | UAT-03 | Should | Sprint 2 |
| BO-1 / performance | NFR-01 | List loads within 2s for up to 100 orders; paginate at 20 | STORY-4 | Scenario: Order list handles a large number of orders | UAT-04 | Could | Sprint 2 |
| BO-1 / privacy | NFR-02 | Order data visible only to the authenticated owner | STORY-1 (security AC) | Scenario: Customer cannot view another customer's orders | UAT-05 | Must | Ready for UAT |

**Coverage check (Sprint 1 Must items):** FR-01, FR-02, FR-03, NFR-02 each have a UAT case.
FR-04 and NFR-01 are intentionally later — they are Should / Could, not gaps.

## 3. How a BA uses this during the sprint

- **Backlog grooming:** if a new story cannot be placed in a row, it is probably
  out of scope or needs a BRD update first.
- **UAT planning:** generate the test pack from the UAT Case column, not from memory.
- **Change impact:** if FR-03 is dropped, STORY-2, its Gherkin and UAT-02 are all
  in question — the matrix shows the blast radius in one glance.
- **Sign-off:** the business owner signs a version of this table with actual
  Pass/Fail results, not a verbal "looks good".

## 4. After UAT (example of updating status)

| UAT Case | Result | Defect | RTM action |
|---|---|---|---|
| UAT-01 | Pass | — | Status → Passed |
| UAT-02 | Pass | — | Status → Passed |
| UAT-05 | Fail | BUG-12 session timeout still shows previous orders | Status → Failed; block sign-off until High defect is fixed and retested |

## 5. Key Takeaway

The RTM is a living index, not a one-off assignment document. The BA updates it
when a requirement, story or test changes — that is what keeps BRD, JIRA and UAT
from drifting apart.
