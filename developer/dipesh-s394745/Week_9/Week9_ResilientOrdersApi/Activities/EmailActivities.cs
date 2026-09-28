using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Temporalio.Activities;
using Week9_ResilientOrdersApi.Models;
using Week9_ResilientOrdersApi.Workflows;

namespace Week9_ResilientOrdersApi.Activities;

// the activity that actually sends the email, called from OrderConfirmationWorkflow
public class EmailActivities
{
    private readonly IConfiguration _config;
    private readonly OrderStore _orderStore;
    private readonly ILogger<EmailActivities> _logger;

    public EmailActivities(IConfiguration config, OrderStore orderStore, ILogger<EmailActivities> logger)
    {
        _config = config;
        _orderStore = orderStore;
        _logger = logger;
    }

    [Activity]
    public async Task SendOrderConfirmationEmailAsync(OrderConfirmationInput input)
    {
        var host = _config["Smtp:Host"] ?? "localhost";
        var port = int.Parse(_config["Smtp:Port"] ?? "2525");

        _logger.LogInformation(
            "Sending order confirmation email for Order {OrderId} to {CustomerEmail} via {SmtpHost}:{SmtpPort}",
            input.OrderId, input.CustomerEmail, host, port);

        var message = new MimeMessage();
        message.From.Add(new MailboxAddress("Orders Team", "orders@week9-demo.local"));
        message.To.Add(new MailboxAddress(input.CustomerName, input.CustomerEmail));
        message.Subject = $"Order Confirmation #{input.OrderId}";
        message.Body = new TextPart("plain")
        {
            Text = $"Hi {input.CustomerName},\n\n" +
                   $"Your order for {input.Quantity} x {input.Product} has been confirmed.\n\n" +
                   $"Order ID: {input.OrderId}\n" +
                   "Thanks for shopping with us!"
        };

        using var client = new SmtpClient();
        await client.ConnectAsync(host, port, SecureSocketOptions.None);
        await client.SendAsync(message);
        await client.DisconnectAsync(true);

        _orderStore.MarkEmailConfirmed(input.OrderId);

        _logger.LogInformation("Order confirmation email sent for Order {OrderId}", input.OrderId);
    }
}
