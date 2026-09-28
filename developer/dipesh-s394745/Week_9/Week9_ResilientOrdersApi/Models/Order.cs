namespace Week9_ResilientOrdersApi.Models;

// A simple class representing one order. Kept in memory for this practice project.
public class Order
{
    public int Id { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
    public DateTime CreatedAtUtc { get; set; } = DateTime.UtcNow;

    // Set to true once the Temporal workflow reports the confirmation email was sent.
    public bool EmailConfirmed { get; set; }
}

// What the client sends us in POST /api/orders.
public class OrderRequest
{
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
}

// A simple in-memory "database" for orders, shared across requests.
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
