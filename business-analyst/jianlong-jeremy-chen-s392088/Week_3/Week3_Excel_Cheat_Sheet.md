# Week 3 — Excel Functions Cheat Sheet (Personal Reference)

Compiled while practising Excel alongside the Power BI / Python exercises this week,
using the same style of order data.

## Lookup & Reference
| Function | Example | Purpose |
|---|---|---|
| VLOOKUP | `=VLOOKUP(A2, Customers!A:B, 2, FALSE)` | Find a value in the first column of a range and return a value from another column |
| INDEX/MATCH | `=INDEX(B:B, MATCH(A2, A:A, 0))` | More flexible alternative to VLOOKUP (works looking left, and is more robust to inserted columns) |

## Aggregation
| Function | Example | Purpose |
|---|---|---|
| SUMIF | `=SUMIF(Region, "NT", Amount)` | Sum values that meet one condition |
| SUMIFS | `=SUMIFS(Amount, Region, "NT", Status, "Completed")` | Sum values that meet multiple conditions |
| COUNTIF / COUNTIFS | `=COUNTIFS(Status, "Pending")` | Count rows meeting one or more conditions |
| AVERAGEIFS | `=AVERAGEIFS(Amount, Status, "Completed")` | Average of values meeting conditions |

## Logic
| Function | Example | Purpose |
|---|---|---|
| IF | `=IF(Amount>300, "Large", "Standard")` | Simple conditional logic |
| Nested IF / IFS | `=IFS(Amount>=300,"Large", Amount>=100,"Medium", TRUE,"Small")` | Multiple conditions without deep nesting |

## Data Tools
- **PivotTable** — drag-and-drop summarisation (e.g. Region as rows, Amount as values,
  Status as filter). Practised building one from the `orders_sample.csv` dataset used in
  the SQL/Python exercises.
- **Conditional Formatting** — used a colour scale on the Amount column to visually
  highlight high vs low value orders.
- **Data Validation** — created a dropdown list for the Status column (Completed /
  Pending / Cancelled) to reduce data entry errors.

## Practice Exercise Completed
Built a PivotTable from `orders_sample.csv`:
- Rows: Region
- Values: Sum of Amount, Count of OrderId
- Filter: Status = Completed

Result matched the SQL `GROUP BY Region` query from Week 2 — good cross-check that both
tools agree on the numbers.
