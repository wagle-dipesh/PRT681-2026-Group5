namespace Week9_ResilientOrdersApi.Workflows;

// The data passed into the Temporal workflow. Must be JSON-serializable,
// since Temporal stores workflow inputs/results durably as JSON.
public class OrderConfirmationInput
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
}
