---

### 📄 File 3: `03_Requirements_Traceability_Matrix_RTM.md`

```markdown
# 03 - End-to-End Requirements Traceability Matrix (RTM)

**Module:** PRT681 / PRT585 - Business Analyst (BA)  
**Tracing Flow:** Business Goals $\rightarrow$ User Stories $\rightarrow$ UI Wireframe Screens $\rightarrow$ UAT Test Scenarios

---

## 1. 3-Line Structure Notes
* **What it is:** A Requirements Traceability Matrix (RTM) is a multi-dimensional grid linking business requirements bi-directionally to downstream designs, code, and test cases.
* **Why it is used:** To guarantee 100% test coverage before release, eliminate gold-plating or scope creep, and quickly identify impact whenever requirements change.
* **Simple Example:** Tracing Strategic Goal `BG-01` (Reduce reorder friction) $\rightarrow$ User Story `US-02` $\rightarrow$ Wireframe `Screen_03` $\rightarrow$ Test Case `TC-UAT-02`.

---

## 2. End-to-End RTM Grid

| Business Goal (BG ID) | User Story (US ID) | INVEST Sizing & Priority | Figma Screen / Component | Acceptance Criteria Scenario | UAT Test Case Reference | Status |
| :--- | :--- | :--- | :--- | :--- | :--- | :---: |
| **BG-01**<br>Increase repeat order rate by 20% | **US-01**<br>As a user, I want to view past orders with summaries | **Must-Have**<br>(3 Story Points)<br>Independent, Estimable | `Screen_01_Order_History`<br>Past order summary card | `Scenario: Successful reorder when all items are in stock` | **TC-UAT-01**<br>Verify past orders display sorted by date descending | Approved |
| **BG-01**<br>Reduce reorder path to under 3 clicks | **US-02**<br>As a user, I want to add past meals to cart in one click | **Must-Have**<br>(5 Story Points)<br>Valuable, Testable | `Screen_02_Item_Availability`<br>`Screen_03_Cart_Checkout` | `Scenario: Successful reorder when all items are in stock` | **TC-UAT-02**<br>Verify items, options, and prices populate cart correctly | Approved |
| **BG-02**<br>Prevent overselling and refund disputes | **US-03**<br>As a user, I want clear notices when items are out of stock | **Must-Have**<br>(5 Story Points)<br>Small, High Value | `Screen_02_Item_Availability`<br>Sold-out badge & alert banner | `Scenario: Reorder containing an out-of-stock item` | **TC-UAT-03**<br>Verify out-of-stock items are excluded and subtotal updates | Approved |
| **BG-03**<br>Reduce accidental order support tickets | **US-04**<br>As a user, I want to self-cancel within 60 seconds | **Should-Have**<br>(5 Story Points)<br>Negotiable, Estimable | `Screen_04_Confirmation`<br>Countdown bar & cancel button | `Scenario: User cancels order within 60s window` | **TC-UAT-04**<br>Verify cancellation halts POS dispatch and triggers refund | Ready for Release |

---

## 3. Traceability Audit Summary
* **Requirement Coverage:** 100% (Every functional requirement maps directly to a Figma screen and a UAT test case).
* **Zero Orphan Deliverables:** No UI component exists without a supporting business goal, and no acceptance criteria lack corresponding test scripts.