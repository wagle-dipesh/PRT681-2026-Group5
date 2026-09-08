# Practice: Mock Backlog + Sprint Planning — "Buy It Again" Feature (Amazon)

## Epic
**EPIC-1: Buy It Again** — Let repeat customers repurchase a past order or item quickly, without misleading them about seller, price, or availability changes.

## Backlog (prioritised, top = highest priority)

| # | Story | Story Points | Priority | Depends on | Notes |
|---|-------|--------------|----------|------------|-------|
| 1 | US1 — View eligible past purchases | 3 | Must Have | — | Read + filter (excludes active Subscribe & Save items); low risk |
| 2 | US2 — Repurchase a single item | 5 | Must Have | US1, catalogue availability check | Core value; needs real-time "still sold?" lookup, not a stale cache |
| 3 | US3 — Seller-change notice | 5 | Must Have | US2 | Estimate raised from 3→5 once we realised "same seller" needs a live check against the *current* listing, not the seller recorded on the original order |
| 4 | US4 — Price-increase notice | 3 | Should Have | US2 | The 15% threshold is a product decision, not an engineering one — needs sign-off before dev starts |
| 5 | US5 — Confirm address & payment | 3 | Must Have | US2 | Reclassified from Should→Must Have after the Week 1 BRD flagged mis-shipped consumables as a known pain point — this isn't a nice-to-have, it's a correctness requirement |
| 6 | US6 — Adjust quantity before confirming | 2 | Should Have | US2 | Mostly frontend cart-state work; low backend risk |

**Prioritisation logic:** US1, US2, US3, and US5 form the minimum *safe* flow — a customer can see eligible items, repurchase one, and is never misled about who's selling it or where it's shipping. US5 was promoted to Must Have during this exercise specifically because the BRD's business rule about address confirmation exists to prevent a real failure mode (shipping to a stale address), not just to improve UX — treating it as optional would contradict the BRD I wrote in Week 1. US4 and US6 improve trust and usability but the feature is shippable and safe without them for a first release.

## Definition of Done (agreed for this backlog)
- Acceptance criteria all pass in a demo environment
- Unit tests exist for the seller-change and price-increase branching logic (the two riskiest, most novel business-rule areas)
- Reviewed against the BRD's business rules line-by-line — no rule silently unimplemented
- Product Owner has seen a demo and explicitly signed off (not just "no objection raised")

## 15-minute mock sprint planning session — notes
*(Presented walkthrough-style to a teammate; summarised here as the task requires.)*

- **Sprint goal proposed:** "Ship a safe, honest single-item repurchase flow (view eligible items → repurchase → transparent seller/address handling) by end of sprint — 'safe' meaning no mis-shipped orders, 'honest' meaning no hidden seller or price changes."
- **Capacity discussion:** assumed a 2-week sprint, team velocity ~10 points/sprint based on similar past features; flagged that this is an assumed number for a new team, not historical data.
- **Selected for sprint:** US1 (3) + US2 (5) = 8 points. US5 (address/payment confirmation, 3 points) was discussed as a strong candidate to include given it's now Must Have, but the team decided against pushing to 11 points on a first sprint with an unproven velocity — deliberately protecting the sprint commitment's credibility over cramming in scope.
- **Discussed as stretch, not committed:** US3 (seller-change notice) was raised as a stretch goal, but — the team flagged the real-time catalogue/seller lookup as an unconfirmed dependency and declined to commit it without checking feasibility first.
- **Risk raised and logged:** the "is this exact ASIN still sold, by whom, right now" lookup was logged as a risk with an owner (Product Owner, to confirm with the Catalogue/Seller platform team) and a due date (before next planning) — this is the kind of dependency that's easy to skip past in planning if no one is explicitly assigned to chase it down.
- **Deferred with reasoning:** US4 (price notice) deferred because the 15% threshold needs a product decision first. US5 was *not* deferred (see above) once the team recognised it as safety-critical rather than a UX nicety — this reclassification only happened because someone re-read the BRD's business rules during planning rather than working from memory of the story titles.
- **Outcome:** Team committed to US1 + US2 (8 points) as sprint scope, with US5 flagged as "should be pulled into this sprint if velocity allows" rather than either fully committed or fully deferred — a useful middle state because "committed vs. stretch" alone doesn't capture a story that's important but not yet estimated against real capacity.

## Retrospective note (self-reflection on this exercise)
Drafting this backlog surfaced a real prioritisation mistake in my first pass: I filed address/payment confirmation as "Should Have" out of habit (that's where it usually sits in a generic checkout flow), without re-checking whether the *specific* business rules for this app changed its importance. They did — Amazon's BRD explicitly calls out mis-shipped consumables as a known pain point, which makes that story safety-critical, not a nicety. The lesson: backlog priority should be re-derived from the current BRD's business rules each time, not carried over from a similar-looking backlog on a different feature.
