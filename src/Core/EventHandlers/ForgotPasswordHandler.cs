using Core.Events;
using Core.Interfaces.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Core.EventHandlers
{
    public class ForgotPasswordHandler : INotificationHandler<ForgotPasswordEvent>
    {
        private readonly ILogger<ForgotPasswordEvent> _logger;
        private readonly IEmailService _emailService;

        public ForgotPasswordHandler(ILogger<ForgotPasswordEvent> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public Task Handle(ForgotPasswordEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending request password notification to {notification.Person.Email} with opt: {notification.Otp}");

            var body = $@"<p>Hello {notification.Person.FirstName},</p> 
                    <p>You have requested to reset your password.</p> 
                    <p>Here is your token to complete the password reset process <b>{notification.Otp}</b></p>
                    <p>Please note that this token is only valid for five minutes</p>";

            _logger.LogInformation("Forgot password email has been sent");
            return _emailService.SendEmailAsync(notification.Person.Email, "Forgot Password", body);
        }
    }

}
