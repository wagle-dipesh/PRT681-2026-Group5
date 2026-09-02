"""
Week 3 - Data Visualisation Tutorial
Purpose: Self-created practice script showing how a BA can quickly explore
         data before building the same charts in Power BI / Tableau.
Dataset: orders_sample.csv (same dataset used in the Week 2 SQL tutorial)

Run with: python Week3_DataViz_Tutorial.py
Requires: pandas, matplotlib  (pip install pandas matplotlib)
"""

import pandas as pd
import matplotlib.pyplot as plt

# ---------------------------------------------------------------------------
# 1. Load the data
# ---------------------------------------------------------------------------
df = pd.read_csv("orders_sample.csv")

# Quick look at the shape and first few rows - always a good first step
print("Dataset shape:", df.shape)
print(df.head())

# ---------------------------------------------------------------------------
# 2. Basic cleaning / filtering
# Only "Completed" orders count as real revenue for this exercise
# ---------------------------------------------------------------------------
completed = df[df["Status"] == "Completed"].copy()

# ---------------------------------------------------------------------------
# 3. Summarise revenue by region (equivalent to a GROUP BY in SQL)
# ---------------------------------------------------------------------------
revenue_by_region = completed.groupby("Region")["Amount"].sum().sort_values(ascending=False)
print("\nRevenue by region:")
print(revenue_by_region)

# ---------------------------------------------------------------------------
# 4. Chart 1 - Bar chart of revenue by region
# This mirrors the kind of bar visual you would build in Power BI
# ---------------------------------------------------------------------------
plt.figure()
revenue_by_region.plot(kind="bar", color="#4472C4")
plt.title("Completed Order Revenue by Region")
plt.xlabel("Region")
plt.ylabel("Total Revenue ($)")
plt.tight_layout()
plt.savefig("revenue_by_region.png")  # saved as evidence for GitHub submission
plt.close()

# ---------------------------------------------------------------------------
# 5. Chart 2 - Revenue per customer (top contributors)
# ---------------------------------------------------------------------------
revenue_by_customer = (
    completed.groupby("CustomerName")["Amount"]
    .sum()
    .sort_values(ascending=False)
)

plt.figure()
revenue_by_customer.plot(kind="bar", color="#70AD47")
plt.title("Completed Order Revenue by Customer")
plt.xlabel("Customer")
plt.ylabel("Total Revenue ($)")
plt.tight_layout()
plt.savefig("revenue_by_customer.png")
plt.close()

# ---------------------------------------------------------------------------
# 6. Chart 3 - Order status breakdown (pie chart)
# Useful for a BA to quickly show stakeholders the health of the order pipeline
# ---------------------------------------------------------------------------
status_counts = df["Status"].value_counts()

plt.figure()
status_counts.plot(kind="pie", autopct="%1.0f%%")
plt.title("Order Status Breakdown")
plt.ylabel("")  # hide default y-label for pie charts
plt.tight_layout()
plt.savefig("order_status_breakdown.png")
plt.close()

# ---------------------------------------------------------------------------
# 7. Takeaway notes (mirrors what these charts would look like in Power BI)
# ---------------------------------------------------------------------------
# - Bar charts: best for comparing totals across categories (region, customer)
# - Pie charts: best for showing proportion of a whole (status split) -
#   use sparingly, only when there are few categories
# - Always ask: "what business question does this chart answer?" before
#   building a visual - this is the core BA skill behind any dashboard.

print("\nCharts saved: revenue_by_region.png, revenue_by_customer.png, "
      "order_status_breakdown.png")
