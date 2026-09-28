# 02 - End-to-End Acceptance Criteria (BDD Gherkin Syntax)

**Module:** PRT681 / PRT585 - Business Analyst (BA)  
**Business Domain:** Historical Meal Reordering, Stock Status Handling & Grace Period Cancellation

---

## 1. 3-Line Structure Notes
* **What it is:** Gherkin is a structured, human-readable domain-specific language for BDD acceptance criteria using the `Given [precondition], When [action], Then [expected outcome]` syntax.
* **Why it is used:** To eliminate ambiguity between business stakeholders, software developers, and QA engineers, turning business rules directly into automated test scripts.
* **Simple Example:** 
  ```gherkin
  Scenario: Exclude unavailable items
    Given a past meal contains a discontinued or sold-out item
    When the user clicks "Reorder"
    Then the system adds only in-stock items to the cart and displays a warning banner.