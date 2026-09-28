namespace Week9_ResilientOrdersApi.Models;

// one order, kept in memory (no database)
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    public bool EmailConfirmed { get; set; } // set true once the workflow sends the email
}

// the request body for POST /api/orders
public class OrderRequest
{
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
}

// in-memory list of orders shared across requests
public class OrderStore
{
    private readonly List<Order> _orders = new();
    private int _nextId = 1;
    private readonly object _lock = new();

    public Order Add(OrderRequest request)
    {
        lock (_lock)
        {
            var order = new Order
            {
                Id = _nextId++,
                CustomerName = request.CustomerName,
                CustomerEmail = request.CustomerEmail,
                Product = request.Product,
                Quantity = request.Quantity,
            };
            _orders.Add(order);
            return order;
        }
    }

    public List<Order> GetAll()
    {
        lock (_lock) return _orders.ToList();
    }

    public Order? GetById(int id)
    {
        lock (_lock) return _orders.FirstOrDefault(o => o.Id == id);
    }

    public void MarkEmailConfirmed(int id)
    {
        lock (_lock)
        {
            var order = _orders.FirstOrDefault(o => o.Id == id);
            if (order != null) order.EmailConfirmed = true;
        }
    }
}
