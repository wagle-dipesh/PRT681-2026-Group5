# Week 3 — Power BI & DAX Concept Notes

## 1. Core Power BI Building Blocks
- **Data source** — imported `orders_sample.csv` as the source table.
- **Data model** — for a real project, this is where relationships between tables
  (e.g. Orders → Customers) are defined; single dataset here so no relationships needed.
- **Report canvas** — where visuals (bar, line, card, table) are placed.
- **Fields pane** — lists all columns/measures available to drag onto visuals.

## 2. Visuals Practised
| Visual | What it shows |
|---|---|
| Bar chart | Revenue by Region (matches the bar chart built in Python/matplotlib) |
| Card | Single-number KPI, e.g. Total Revenue |
| Table | Raw order-level detail with filters applied |
| Slicer | Interactive filter for Region / Status, applied across all visuals on the page |

## 3. Basic DAX Measures Written

```dax
Total Revenue = SUM(Orders[Amount])

Completed Revenue =
CALCULATE(
    SUM(Orders[Amount]),
    Orders[Status] = "Completed"
)

Average Order Value =
DIVIDE([Total Revenue], COUNTROWS(Orders), 0)
```

**Notes on DAX vs Excel:**
- `SUM()` behaves like Excel's SUM, but works within the Power BI data model context.
- `CALCULATE()` is the most important DAX function — it changes the filter context
  a measure is evaluated in (similar in spirit to SUMIFS in Excel, but far more flexible).
- `DIVIDE()` is preferred over `/` because it safely handles division by zero
  (returns the third argument, here `0`, instead of an error).

## 4. Key Takeaway
Power BI's DAX measures are conceptually close to the SQL aggregate queries practised
earlier this week (`SUM`, `GROUP BY`, filtering) — the main new idea is **filter
context**: a measure's result changes automatically depending on which slicers/filters
are applied on the report page, without rewriting the formula.
