namespace Week9_ResilientOrdersApi.Workflows;

// data passed into the workflow
public class OrderConfirmationInput
{
    public int OrderId { get; set; }
    public string CustomerName { get; set; } = "";
    public string CustomerEmail { get; set; } = "";
    public string Product { get; set; } = "";
    public int Quantity { get; set; }
}
