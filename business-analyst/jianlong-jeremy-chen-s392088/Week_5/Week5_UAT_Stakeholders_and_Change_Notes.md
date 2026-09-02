# Week 5 — UAT, Stakeholders & Change Control

Week 4 turned the "My Orders" idea into a BRD, user stories, Gherkin scenarios and a
JIRA backlog. This week closes the loop: who owns what, how requirements are proved,
and how the BA supports testing and change after development starts.

The running example is still **Customer Order Tracking Enhancement**.

## 1. Why this week matters for a BA

A requirement that is written but never tested is not a delivered requirement.
Week 5 skills sit between "the story is in JIRA" and "the business can sign off":

| Skill | What it answers |
|---|---|
| Stakeholder map / RACI | Who decides, who is consulted, who only needs to be informed |
| Requirements Traceability Matrix (RTM) | Every FR/NFR maps to a story and a test |
| UAT | Real users confirm the solution solves the original problem |
| Defect logging | Failed tests become trackable work, not hallway conversations |
| Change control | New requests are assessed against scope, not silently added |
| Release notes / handover | Support and users know what shipped |

## 2. Stakeholder Analysis

### 2.1 Power / Interest grid

| Stakeholder | Power | Interest | How the BA engages |
|---|---|---|---|
| Head of E-commerce | High | High | Manage closely — owns business outcomes (call volume, CSAT) |
| Customer Support Manager | Medium | High | Keep informed and involved in UAT — closest to the pain |
| Development Team Lead | Medium | High | Collaborate weekly — feasibility, estimates, defects |
| QA / Tester | Low | High | Keep informed — they turn Gherkin into executable tests |
| End customer (user) | Low | High | Sample in UAT; do not design from one loud voice |
| Finance / Legal | High | Low | Keep satisfied — only if payments or PII rules change |

**Rule of thumb:** high power + high interest people must see prototypes and UAT
results. High power + low interest people need a short status, not a 20-page BRD.

### 2.2 RACI for "My Orders"

R = Responsible (does the work), A = Accountable (one owner), C = Consulted, I = Informed.

| Activity | BA | Product Owner / Head of E-comm | Dev Lead | QA | Support Manager | End user |
|---|---|---|---|---|---|---|
| Elicit & document requirements | R | A | C | I | C | C |
| Write user stories & AC | R | A | C | C | I | I |
| Build the feature | C | I | R / A | I | I | I |
| Write UAT scripts | R | C | I | C | C | I |
| Execute UAT | C | A (sign-off) | I | C | R | R |
| Log / triage defects | R | C | C | C | I | I |
| Release notes & handover | R | A | C | I | C | I |

**Key principle:** there should be only **one A** per row. Two Accountable people
usually means nobody is actually accountable.

## 3. UAT vs other testing (BA view)

| Type | Who typically runs it | Question it answers |
|---|---|---|
| Unit / integration | Developers | Does this code work in isolation / with other code? |
| System / QA testing | QA | Does the build match the acceptance criteria? |
| **UAT** | Business users, facilitated by the BA | Does this solve the business problem in a real workflow? |

UAT is **not** a second QA cycle. If QA has not passed the build, do not start UAT —
users will spend their time finding crashes instead of validating the process.

### 3.1 Entry criteria (when UAT may start)

- Sprint 1 stories (STORY-1, STORY-2) meet the Definition of Done from Week 4.
- QA has passed the Gherkin scenarios for those stories.
- A staging environment exists with realistic (or anonymised) order data.
- UAT scripts and testers are confirmed.
- Known open defects are documented so users are not surprised.

### 3.2 Exit criteria (when UAT is done)

- All Must-have UAT cases passed (or failed cases have an agreed defect + target fix).
- No open **Critical / High** defects.
- Business owner has signed the UAT sign-off (see test plan).
- RTM updated with actual test results.

## 4. Defect logging practice

When a UAT case fails, log a defect in JIRA rather than describing it in chat.
A usable defect has:

1. **Title** — what failed, in one line.
2. **Steps to reproduce** — numbered, starting from a known state.
3. **Expected vs actual** — tied back to an acceptance criterion.
4. **Severity** — how badly the product is broken.
5. **Priority** — how soon it should be fixed (business call).
6. **Environment / data** — staging URL, user, order ID.

### Severity vs priority

| | Meaning | Example on My Orders |
|---|---|---|
| Severity | Technical / user impact | Customer B can see Customer A's orders → Critical |
| Priority | When we will fix it | Same issue is also P1 because it is a Must NFR |

A cosmetic typo can be High priority before a demo, but Low severity.

### Sample defect (practice)

```
Title: Order list shows another customer's orders after session timeout

Environment: Staging, Chrome, test user customer.a@example.com
Story / AC: STORY-1 / NFR-02 (orders visible only to the owner)

Steps:
1. Log in as Customer A and open My Orders.
2. Leave the browser idle until the session expires (or clear the auth cookie).
3. Without logging in again, refresh the My Orders page.

Expected: user is redirected to login; no order data is shown.
Actual: the previous order list remains visible for ~10 seconds, including
        order numbers that belong to Customer A, before redirect.

Severity: High
Priority: Must-fix before UAT sign-off
```

## 5. Change control (scope after the BRD is signed)

New ideas will appear during UAT ("can we also cancel the order from this page?").
That request is **out of scope** in the Week 4 BRD. The BA does not say "no" forever;
the BA makes the impact visible:

1. Capture the request as a **Change Request** (not a silent story in the current sprint).
2. Trace impact: BRD scope, stories, NFRs, tests, release date.
3. MoSCoW with the Product Owner — often it becomes a Could for a later sprint.
4. If accepted, update BRD, RTM and backlog together so they do not drift apart.

**Change request one-liner (practice):**
> CR-01: Allow customers to cancel a Pending order from the order detail page.
> Impact: new story (~5 points), warehouse process change, extra UAT cases.
> Decision: Out of scope for Sprint 1–2; park as Could for Phase 2.

## 6. Key Takeaways

- RACI fails when two people are Accountable; pick one owner for sign-off.
- UAT proves business value, not code quality — keep QA and UAT separate.
- Every Must requirement should appear in the RTM **and** in a UAT script.
- Defects need expected/actual and a story link; severity and priority are different.
- Scope change is allowed, but only through an explicit change request that updates
  the BRD, backlog and tests together.
