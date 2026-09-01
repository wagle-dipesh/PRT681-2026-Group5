# Week 5 — UAT Test Plan (My Orders)

Converted from Week 4 Gherkin so a business user can execute tests without reading
developer tickets. Same feature: customer self-service order history.

## 1. Plan summary

| Field | Detail |
|---|---|
| Feature | My Orders list + order detail |
| Build under test | Sprint 1 — STORY-1, STORY-2 (Must) |
| Environment | Staging, with seeded customers and orders |
| Facilitator | BA |
| Testers | Customer Support Manager + 2 support agents (as proxy end users) |
| Sign-off | Head of E-commerce |
| Entry criteria | QA passed; staging data ready; no open Critical defects |
| Exit criteria | All Must cases passed; no open High/Critical defects; sign-off recorded |

Sprint 2 cases (UAT-03 empty state, UAT-04 pagination) are listed but not required
for Sprint 1 sign-off.

## 2. Test data (seeded)

| User | Password (staging only) | Orders in data |
|---|---|---|
| customer.a@example.com | (staging secret) | 3 orders: #101 Completed, #104 Pending, #109 Pending |
| customer.b@example.com | (staging secret) | 2 orders belonging only to B |
| customer.empty@example.com | (staging secret) | 0 orders (Sprint 2) |
| customer.heavy@example.com | (staging secret) | 60 orders (Sprint 2) |

Testers must not use production accounts.

## 3. UAT scripts

Each case: one tester, one Pass/Fail, comments for anything unexpected.

### UAT-01 — View my order history
**Traces to:** FR-01, FR-02, STORY-1

| # | Step | Expected |
|---|---|---|
| 1 | Log in as `customer.a@example.com` | Account home loads |
| 2 | Open **My Orders** | Page title is My Orders |
| 3 | Read the list | Three of *my* orders appear; most recent first (#109, #104, #101) |
| 4 | Check columns | Each row shows order number, date, amount, status |
| 5 | Confirm statuses | #101 Completed; #104 and #109 Pending |

**Pass / Fail:** ______ &nbsp;&nbsp; **Tester:** ______ &nbsp;&nbsp; **Date:** ______

### UAT-02 — Open order detail
**Traces to:** FR-03, STORY-2

| # | Step | Expected |
|---|---|---|
| 1 | From UAT-01, click order #104 | Detail page opens |
| 2 | Check header | Order number, date, amount, status = Pending |
| 3 | Check lines | Item names and quantities match the seeded order |
| 4 | Use Back / breadcrumb | Returns to the list; list still correct |

**Pass / Fail:** ______ &nbsp;&nbsp; **Tester:** ______ &nbsp;&nbsp; **Date:** ______

### UAT-03 — Empty state (Sprint 2 / Should)
**Traces to:** FR-04, STORY-3

| # | Step | Expected |
|---|---|---|
| 1 | Log in as `customer.empty@example.com` | Account home loads |
| 2 | Open **My Orders** | Message: "You have no orders yet." |
| 3 | Confirm layout | No blank table and no error page |

**Pass / Fail:** ______ &nbsp;&nbsp; **Tester:** ______ &nbsp;&nbsp; **Date:** ______

### UAT-04 — Pagination / performance (Sprint 2 / Could)
**Traces to:** NFR-01, STORY-4

| # | Step | Expected |
|---|---|---|
| 1 | Log in as `customer.heavy@example.com` | Account home loads |
| 2 | Open **My Orders** and note load time | Page usable within 2 seconds |
| 3 | Count rows on page 1 | 20 orders |
| 4 | Go to page 2 | Next 20 orders; no duplicates from page 1 |

**Pass / Fail:** ______ &nbsp;&nbsp; **Tester:** ______ &nbsp;&nbsp; **Date:** ______

### UAT-05 — Cannot see another customer's orders (Must / security)
**Traces to:** NFR-02

| # | Step | Expected |
|---|---|---|
| 1 | Log in as Customer A, open any order, copy the detail URL | URL contains an order id |
| 2 | Log out | Login page shown |
| 3 | Log in as Customer B | B's home loads |
| 4 | Paste A's order URL | Access denied / error; A's items and amounts are **not** shown |
| 5 | Open B's My Orders | Only B's orders appear |

**Pass / Fail:** ______ &nbsp;&nbsp; **Tester:** ______ &nbsp;&nbsp; **Date:** ______

## 4. Bug triage during UAT

| If the tester sees… | BA action |
|---|---|
| Steps cannot be followed (blocked) | Log defect, mark case **Blocked**, do not Fail yet |
| Wrong data but page works | Fail the case, log defect, link to FR / story |
| Typo / colour / wording | Log as Low severity; do not block Must sign-off unless it changes meaning |
| Security leak (UAT-05) | Critical; stop UAT for that build; retest after fix |

## 5. Sign-off

Sprint 1 UAT is accepted when UAT-01, UAT-02 and UAT-05 are Pass and no High/Critical
defects remain open.

| Role | Name | Date | Decision (Accept / Reject) |
|---|---|---|---|
| UAT facilitator (BA) | | | |
| Customer Support Manager | | | |
| Head of E-commerce (Accountable) | | | |

## 6. Key Takeaway

Writing UAT as numbered business steps (not Gherkin) is what lets non-technical
testers execute the same scenarios the developers already coded against.
The BA's job is to keep that translation exact — if the script drifts from the AC,
sign-off no longer means the BRD was met.
