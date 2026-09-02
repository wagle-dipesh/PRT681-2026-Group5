/* =========================================================================
   Week 2 - SQL Basics Tutorial
   Purpose: Self-created practice script covering the SQL fundamentals a
            Business Analyst needs in order to query and interrogate data.
   Tool used: SQL Server Management Studio (SSMS)
   ========================================================================= */

/* -------------------------------------------------------------------------
   1. CREATE SAMPLE TABLES
   We build a small "Orders" database to practise querying against.
   ------------------------------------------------------------------------- */

CREATE TABLE Customers (
    CustomerId   INT PRIMARY KEY,      -- Unique identifier for each customer
    CustomerName VARCHAR(100) NOT NULL,
    Region       VARCHAR(50)
);

CREATE TABLE Orders (
    OrderId      INT PRIMARY KEY,      -- Unique identifier for each order
    CustomerId   INT NOT NULL,         -- Foreign key linking back to Customers
    OrderDate    DATE NOT NULL,
    Amount       DECIMAL(10,2) NOT NULL,
    Status       VARCHAR(20),          -- e.g. 'Completed', 'Pending', 'Cancelled'
    FOREIGN KEY (CustomerId) REFERENCES Customers(CustomerId)
);

/* -------------------------------------------------------------------------
   2. INSERT SAMPLE DATA
   Small dataset so query results are easy to check by eye.
   ------------------------------------------------------------------------- */

INSERT INTO Customers (CustomerId, CustomerName, Region) VALUES
    (1, 'Alice Johnson', 'NT'),
    (2, 'Ben Carter',    'QLD'),
    (3, 'Chloe Nguyen',  'NT'),
    (4, 'David Smith',   'VIC');

INSERT INTO Orders (OrderId, CustomerId, OrderDate, Amount, Status) VALUES
    (101, 1, '2026-07-01', 250.00, 'Completed'),
    (102, 1, '2026-07-05', 120.50, 'Completed'),
    (103, 2, '2026-07-03',  75.00, 'Cancelled'),
    (104, 3, '2026-07-04', 300.00, 'Pending'),
    (105, 3, '2026-07-06',  60.00, 'Completed'),
    (106, 4, '2026-07-07', 410.00, 'Completed');

/* -------------------------------------------------------------------------
   3. BASIC SELECT
   Retrieve all columns and all rows from a table.
   ------------------------------------------------------------------------- */
SELECT * FROM Customers;

/* -------------------------------------------------------------------------
   4. SELECT with WHERE
   Filter rows based on a condition - here, only customers from the NT region.
   ------------------------------------------------------------------------- */
SELECT CustomerName, Region
FROM Customers
WHERE Region = 'NT';

/* -------------------------------------------------------------------------
   5. SELECT with WHERE + comparison operators
   Find all completed orders above $100.
   ------------------------------------------------------------------------- */
SELECT OrderId, Amount, Status
FROM Orders
WHERE Status = 'Completed' AND Amount > 100;

/* -------------------------------------------------------------------------
   6. ORDER BY
   Sort orders by amount, highest first.
   ------------------------------------------------------------------------- */
SELECT OrderId, Amount
FROM Orders
ORDER BY Amount DESC;

/* -------------------------------------------------------------------------
   7. Aggregate functions: COUNT, SUM, AVG
   Common questions a BA is asked: "how many orders?" "total revenue?"
   ------------------------------------------------------------------------- */
SELECT
    COUNT(*)        AS TotalOrders,       -- number of rows
    SUM(Amount)      AS TotalRevenue,      -- sum of Amount column
    AVG(Amount)      AS AverageOrderValue  -- average of Amount column
FROM Orders
WHERE Status = 'Completed';

/* -------------------------------------------------------------------------
   8. GROUP BY
   Summarise total revenue per customer - a typical BA reporting requirement.
   ------------------------------------------------------------------------- */
SELECT
    c.CustomerName,
    SUM(o.Amount) AS TotalSpent
FROM Orders o
JOIN Customers c ON o.CustomerId = c.CustomerId   -- basic INNER JOIN
WHERE o.Status = 'Completed'
GROUP BY c.CustomerName
ORDER BY TotalSpent DESC;

/* -------------------------------------------------------------------------
   9. JOIN across two tables
   Show each order together with the customer's name and region.
   This is the kind of query a BA might use to build a reporting dataset.
   ------------------------------------------------------------------------- */
SELECT
    o.OrderId,
    c.CustomerName,
    c.Region,
    o.OrderDate,
    o.Amount,
    o.Status
FROM Orders o
INNER JOIN Customers c ON o.CustomerId = c.CustomerId;

/* -------------------------------------------------------------------------
   10. Practice takeaway notes
   - WHERE filters rows before grouping; HAVING filters after grouping.
   - Always alias tables (o, c) once you start joining - it keeps queries readable.
   - Aggregate functions (SUM/COUNT/AVG) are the basis of most BA reporting asks.
   ------------------------------------------------------------------------- */
