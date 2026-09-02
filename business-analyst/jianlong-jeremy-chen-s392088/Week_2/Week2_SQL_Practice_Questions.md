# Week 2 — SQL Practice Questions (Self-Test)

Written against the `Customers` / `Orders` tables from `Week2_SQL_Tutorial.sql`.
I wrote my own answer under each question, then checked it by running it in SSMS.

---

**Q1. List all customers from the 'NT' region.**
```sql
SELECT CustomerName FROM Customers WHERE Region = 'NT';
```

**Q2. Find the total number of orders that are still 'Pending'.**
```sql
SELECT COUNT(*) AS PendingOrders FROM Orders WHERE Status = 'Pending';
```

**Q3. List each customer's total completed order value, highest first.**
```sql
SELECT c.CustomerName, SUM(o.Amount) AS TotalSpent
FROM Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId
WHERE o.Status = 'Completed'
GROUP BY c.CustomerName
ORDER BY TotalSpent DESC;
```

**Q4. Which customers have never placed an order?**
```sql
SELECT c.CustomerName
FROM Customers c
LEFT JOIN Orders o ON c.CustomerId = o.CustomerId
WHERE o.OrderId IS NULL;
```

**Q5. What is the average order value, rounded to 2 decimal places?**
```sql
SELECT ROUND(AVG(Amount), 2) AS AvgOrderValue FROM Orders;
```

**Q6. Show the single largest order (customer name, amount, date).**
```sql
SELECT TOP 1 c.CustomerName, o.Amount, o.OrderDate
FROM Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId
ORDER BY o.Amount DESC;
```

**Q7. Which region generated the most completed revenue?**
```sql
SELECT TOP 1 c.Region, SUM(o.Amount) AS Revenue
FROM Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId
WHERE o.Status = 'Completed'
GROUP BY c.Region
ORDER BY Revenue DESC;
```

## Reflection
Writing my own questions (instead of only following a tutorial) made me think about
what a stakeholder would actually ask for in a reporting meeting — most of these map
directly to real BA reporting requests ("who are our top customers", "what's our
average order size", "which region is underperforming").
