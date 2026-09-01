"""
Week 3 (Part 2) - Trend & Correlation Visualisation Tutorial
Purpose: Follow-up practice script covering line charts (trend over time)
         and scatter plots (relationship between two variables) - the two
         chart types not covered in Week3_DataViz_Tutorial.py.
Dataset: monthly_sales.csv (self-created for this exercise)

Run with: python Week3_DataViz_Tutorial_Part2.py
Requires: pandas, matplotlib
"""

import pandas as pd
import matplotlib.pyplot as plt

# ---------------------------------------------------------------------------
# 1. Load the data
# ---------------------------------------------------------------------------
df = pd.read_csv("monthly_sales.csv", parse_dates=["Month"])
print(df)

# ---------------------------------------------------------------------------
# 2. Chart 1 - Line chart showing revenue trend over time
# A BA would use this to spot seasonality or growth/decline patterns.
# ---------------------------------------------------------------------------
plt.figure()
plt.plot(df["Month"], df["Revenue"], marker="o", color="#4472C4")
plt.title("Monthly Revenue Trend")
plt.xlabel("Month")
plt.ylabel("Revenue ($)")
plt.xticks(rotation=45)
plt.tight_layout()
plt.savefig("monthly_revenue_trend.png")
plt.close()

# ---------------------------------------------------------------------------
# 3. Chart 2 - Scatter plot: Marketing Spend vs Revenue
# Used to visually check whether two variables appear related - a common
# first step before a stakeholder asks "does marketing spend drive sales?"
# ---------------------------------------------------------------------------
plt.figure()
plt.scatter(df["MarketingSpend"], df["Revenue"], color="#ED7D31")
plt.title("Marketing Spend vs Revenue")
plt.xlabel("Marketing Spend ($)")
plt.ylabel("Revenue ($)")
plt.tight_layout()
plt.savefig("marketing_spend_vs_revenue.png")
plt.close()

# ---------------------------------------------------------------------------
# 4. Quick correlation check
# ---------------------------------------------------------------------------
correlation = df["MarketingSpend"].corr(df["Revenue"])
print(f"\nCorrelation between Marketing Spend and Revenue: {correlation:.2f}")

# ---------------------------------------------------------------------------
# 5. Takeaway notes
# ---------------------------------------------------------------------------
# - Line charts: best for showing a trend across an ordered dimension (time).
# - Scatter plots: best for spotting a possible relationship between two
#   numeric variables - but correlation is NOT the same as causation, which
#   is an important caveat to raise with stakeholders.
# - A correlation close to +1 or -1 suggests a strong relationship; close
#   to 0 suggests little to no linear relationship.

print("\nCharts saved: monthly_revenue_trend.png, marketing_spend_vs_revenue.png")
