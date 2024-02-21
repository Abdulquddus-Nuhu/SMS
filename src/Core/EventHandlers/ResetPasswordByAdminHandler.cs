using Core.Events;
using Core.Interfaces.Infrastructure;
using Mailjet.Client.Resources;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.EventHandlers
{
    public class ResetPasswordByAdminHandler : INotificationHandler<ResetPasswordByAdminEvent>
    {
        private readonly ILogger<ResetPasswordByAdminEvent> _logger;
        private readonly IEmailService _emailService;

        public ResetPasswordByAdminHandler(ILogger<ResetPasswordByAdminEvent> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public Task Handle(ResetPasswordByAdminEvent notification, CancellationToken cancellationToken)
        {
            var body = $"Hello {notification.Person.UserName},\n\nYour password has been reset by the admin. Your new password is: {notification.newPassword}\n\nPlease change your password as soon as possible for security reasons.\n\nThank you.";


            _logger.LogInformation("Newly set password for user has been sent to user email");
            return _emailService.SendEmailAsync(notification.Person.Email, "Your password has been reset by admin", body);
        }
    }
}
