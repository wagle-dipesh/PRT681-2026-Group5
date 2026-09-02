/* =========================================================================
   Week 2 (Part 2) - Intermediate SQL Tutorial
   Purpose: Follow-up practice script covering HAVING, subqueries, views
            and simple string/date functions - building on the basics
            script (Week2_SQL_Tutorial.sql).
   Uses the same Customers / Orders tables created in Part 1.
   ========================================================================= */

/* -------------------------------------------------------------------------
   1. HAVING - filtering AFTER aggregation
   Difference from WHERE: WHERE filters rows before grouping,
   HAVING filters groups after aggregation.
   Example: find customers who have spent more than $200 in total.
   ------------------------------------------------------------------------- */
SELECT
    c.CustomerName,
    SUM(o.Amount) AS TotalSpent
FROM Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId
WHERE o.Status = 'Completed'
GROUP BY c.CustomerName
HAVING SUM(o.Amount) > 200
ORDER BY TotalSpent DESC;

/* -------------------------------------------------------------------------
   2. Subquery in WHERE clause
   Find all customers who have placed at least one order over $300.
   ------------------------------------------------------------------------- */
SELECT CustomerName
FROM Customers
WHERE CustomerId IN (
    SELECT CustomerId
    FROM Orders
    WHERE Amount > 300
);

/* -------------------------------------------------------------------------
   3. Subquery in SELECT clause
   Show every customer alongside their total number of orders (including
   customers with zero orders, using a correlated subquery).
   ------------------------------------------------------------------------- */
SELECT
    c.CustomerName,
    (SELECT COUNT(*) FROM Orders o WHERE o.CustomerId = c.CustomerId) AS OrderCount
FROM Customers c;

/* -------------------------------------------------------------------------
   4. LEFT JOIN vs INNER JOIN
   INNER JOIN only returns matches; LEFT JOIN keeps all rows from the left
   table even if there's no match on the right - important for BAs to
   understand when reports look like they're "missing" data.
   ------------------------------------------------------------------------- */
-- INNER JOIN: only customers who HAVE placed an order
SELECT c.CustomerName, o.OrderId
FROM Customers c
INNER JOIN Orders o ON c.CustomerId = o.CustomerId;

-- LEFT JOIN: ALL customers, even ones with no orders (OrderId will be NULL)
SELECT c.CustomerName, o.OrderId
FROM Customers c
LEFT JOIN Orders o ON c.CustomerId = o.CustomerId;

/* -------------------------------------------------------------------------
   5. Simple date functions
   Extract month/year from OrderDate - useful for monthly reporting asks.
   ------------------------------------------------------------------------- */
SELECT
    OrderId,
    OrderDate,
    YEAR(OrderDate)  AS OrderYear,
    MONTH(OrderDate) AS OrderMonth
FROM Orders;

/* -------------------------------------------------------------------------
   6. CASE statement
   Bucket orders into simple size categories - shows how business rules
   can be expressed directly in SQL.
   ------------------------------------------------------------------------- */
SELECT
    OrderId,
    Amount,
    CASE
        WHEN Amount >= 300 THEN 'Large'
        WHEN Amount >= 100 THEN 'Medium'
        ELSE 'Small'
    END AS OrderSize
FROM Orders;

/* -------------------------------------------------------------------------
   7. Creating a VIEW
   A view saves a query as a virtual table - useful for giving a BA/report
   writer a simplified, reusable dataset without re-writing the JOIN logic
   every time.
   ------------------------------------------------------------------------- */
CREATE VIEW vw_CompletedOrdersSummary AS
SELECT
    c.CustomerName,
    c.Region,
    o.OrderId,
    o.OrderDate,
    o.Amount
FROM Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId
WHERE o.Status = 'Completed';

-- Now this can be queried like a normal table:
SELECT * FROM vw_CompletedOrdersSummary WHERE Region = 'NT';

/* -------------------------------------------------------------------------
   8. Practice takeaway notes
   - HAVING is to GROUP BY what WHERE is to individual rows.
   - LEFT JOIN is the one BAs reach for most often when a report needs to
     show "everyone", not just "everyone with a match".
   - Views are a simple way to hand a report writer a clean, pre-joined
     dataset without exposing the full schema complexity.
   ------------------------------------------------------------------------- */
