using Temporalio.Activities;
using TaskManagerApi.Models;
using TaskManagerApi.Services;

namespace TaskManagerApi.Workflows;

public class EmailActivities
{
    private readonly IEmailService _emailService;
    private readonly ILogger<EmailActivities> _logger;

    public EmailActivities(
        IEmailService emailService,
        ILogger<EmailActivities> logger)
    {
        _emailService = emailService;
        _logger = logger;
    }

    [Activity]
    public async Task SendEmailAsync(EmailRequest request)
    {
        _logger.LogInformation(
            "Temporal email activity started for {Recipient}",
            request.Recipient);

        await _emailService.SendEmailAsync(
            request.Recipient,
            request.Subject,
            request.Body);

        _logger.LogInformation(
            "Temporal email activity completed for {Recipient}",
            request.Recipient);
    }
}