# Practice: Business Requirements Document — "Buy It Again" Feature (Amazon)

**Author:** Tianhui Ke — Week 1 BA practice

## Persona
**Marcus, 41, parent of two.** Buys household consumables on Amazon roughly every 3 weeks — nappies, dish soap, printer paper, protein bars. He doesn't browse; he searches or, better, just wants to repeat the exact same order without hunting for the product page again. He's price-sensitive on staples but not brand-flexible (his kids won't take a different nappy brand). He shops from a laptop on weekends, usually with a toddler interrupting him, so speed and low error-tolerance both matter.

## 1. Business Need
Customers who buy the same consumables repeatedly currently have to search for each product again, re-check it's the same variant/size, and rebuild their basket — a task that's disproportionately annoying for low-consideration, repeat-purchase categories (household goods, groceries, pet supplies) where the customer already knows exactly what they want. This friction pushes habitual purchases toward competitors with a faster repurchase path, or toward Subscribe & Save even when the customer doesn't actually want a fixed recurring schedule.

## 2. Objective
Let a customer repurchase any past order (or a single past item) in one or two actions, preserving exact product variant (size/flavour/colour) where possible, while being transparent about price and seller changes — increasing repeat-purchase rate without creating support complaints about "this isn't what I ordered last time."

## 3. Scope
**In scope:**
- Viewing a "Buy Again" list of eligible past purchases
- Re-adding a single past item to cart, or an entire past order
- Handling a changed seller (many Amazon listings are fulfilled by third-party sellers, not Amazon itself), a discontinued variant, or a price change
- Adjusting quantity before checkout

**Out of scope (future iterations):**
- Auto-detecting "you're due to run out" timing (a machine-learning prediction feature, not a rules-based one — separate epic)
- Modifying Subscribe & Save schedules (that's an existing, separate feature)
- Repurchasing an item that has been removed from Amazon entirely (handled as "no longer available", not a similar-item recommendation — recommending alternatives is a separate discovery feature, not a repurchase feature)

## 4. Stakeholders
| Stakeholder | Interest | Influence |
|---|---|---|
| Customer (e.g. Marcus) | Wants the *exact same item*, fast, with no surprises | Low individually, high in aggregate (repeat-purchase revenue) |
| Third-party seller | Wants to keep the "Buy Again" listing pointed at their storefront, not lose it silently to a different seller of the same product | Medium — a seller can go out of stock or be replaced as the "featured" seller on a listing |
| Category/Retail team | Owns repeat-purchase rate as a business metric for consumables | High |
| Customer Support | Handles disputes when a repurchase doesn't match what the customer expected | Medium |
| Fulfilment/Logistics | Needs the reorder to route to whichever seller/warehouse is actually fulfilling it now, not a stale one | Medium |

## 5. Business Rules
- "Buy Again" only surfaces items with at least one completed (delivered, not cancelled/returned) past order.
- If the exact product variant (same size/flavour/ASIN) is discontinued, the item is marked "no longer available" — the system does not silently substitute a similar variant without explicit customer confirmation.
- If the same product is now fulfilled by a different seller than the original order, the customer sees a "Sold by [new seller]" notice before adding to cart — sold-by changes are common on Amazon and are a frequent source of "why did this come from someone else" complaints if hidden.
- Price shown is always the current live price, never the historical price paid; if the new price is more than 15% higher than last paid, a "price has increased" notice is shown (15% chosen here as a stricter threshold than a discretionary-purchase app, since consumable repurchases are more price-sensitive per the persona).
- Repurchase always uses the customer's default shipping address and default payment method, but both must be explicitly confirmed on the review screen, not silently reused — mis-shipped consumables to an old address is a known pain point for movers/multi-address households.
- Items currently in an active Subscribe & Save schedule are excluded from "Buy Again" (they're already being auto-reordered; showing them again would be confusing and could cause duplicate orders).

## 6. Success Metrics
- % of eligible customers (2+ historical orders of the same item) who use "Buy Again" at least once in a rolling 90-day window
- Repurchase-to-delivery time (from tapping "Buy Again" to order confirmation) vs. the standard search-and-buy path
- Rate of "wrong seller" / "wrong variant" customer support contacts tied to a "Buy Again" order, tracked as a guardrail metric — this feature should not *increase* support load even as it increases usage

## 7. Assumptions & Open Questions
- Assumes the catalogue/seller service can answer "is this exact ASIN still sold, and by whom" at the time "Buy Again" is opened, not from a stale cache — flagged as a dependency to confirm before estimation.
- Open question: for a household with shared accounts (e.g. Marcus and his partner both order on the same account), should "Buy Again" surface *anyone's* past orders on the account, or does that create a privacy/surprise concern (e.g. surfacing a gift purchase)? Not resolved in this BRD — flagged for a stakeholder workshop.

---

## User Stories (with acceptance criteria)

**US1 — View eligible past purchases**
As Marcus, a repeat customer, I want to see a "Buy Again" list of things I've ordered before, so that I can quickly find a consumable I need to restock.
- *Given* I have at least one delivered order in the last 12 months, *when* I open "Buy Again", *then* I see past purchases sorted by most recently ordered, each showing product name, variant, and last price paid.
- *Given* an item is currently on an active Subscribe & Save schedule, *when* I view "Buy Again", *then* that item is excluded from the list (business rule above).

**US2 — Repurchase a single item**
As Marcus, I want to re-add one specific past item to my cart, so that I don't have to search for it or re-verify it's the right variant.
- *Given* the exact variant is still sold and in stock, *when* I tap "Buy Again" on that item, *then* it's added to my cart with the same variant and current price, and I see a confirmation toast.
- *Given* the variant is discontinued, *when* I tap "Buy Again" on that item, *then* I see "This item is no longer available" and it is not added to the cart — no silent substitution.

**US3 — Be notified transparently of a seller change**
As Marcus, I want to know if a repurchased item now ships from a different seller, so that I'm not confused if the packaging/delivery experience differs from last time.
- *Given* the same product/variant is now fulfilled by a different seller than my original order, *when* I add it via "Buy Again", *then* I see a "Now sold by [Seller Name]" notice on the cart line before checkout.
- *Given* the seller is unchanged, *when* I add it via "Buy Again", *then* no seller notice is shown (avoids unnecessary noise for the common case).

**US4 — Understand price changes before paying**
As Marcus, I want to see if the price has gone up significantly since I last bought this, so that I can decide whether to still buy it or look for an alternative.
- *Given* the current price is more than 15% higher than what I paid last time, *when* I reach cart review, *then* I see a "Price has increased since your last order" notice showing both the old and new price.
- *Given* the price increase is 15% or less, *when* I reach cart review, *then* only the current price is shown without a special notice.

**US5 — Confirm shipping address and payment before repurchasing**
As Marcus, I want to explicitly confirm where this repurchase is going and how it's paid, so that a stale saved address doesn't silently receive my order.
- *Given* I initiate a "Buy Again" order, *when* I reach checkout, *then* my default address and payment method are pre-filled but shown for explicit confirmation, not auto-submitted.
- *Given* I change the address on this repurchase, *when* I confirm the order, *then* only this order uses the new address — my saved default address is unchanged unless I explicitly update it.

**US6 — Adjust quantity before confirming**
As Marcus, I want to change the quantity when repurchasing, so that I can buy more or less than last time without starting over.
- *Given* an item has been added via "Buy Again", *when* I change its quantity on the cart screen, *then* the line total and cart total update immediately.
- *Given* I set the quantity to zero, *when* I confirm the removal, *then* the item is removed from the cart entirely.

## Definition of Ready (for these stories to enter a sprint)
- Acceptance criteria reviewed by at least one teammate
- Confirmed with the Catalogue/Seller platform team that real-time "is this ASIN still sold, by whom" lookups are feasible within the required latency
- UX has a rough wireframe for the "seller changed" and "price increased" notices (US3/US4), since these are the two most novel, easy-to-get-wrong interactions
