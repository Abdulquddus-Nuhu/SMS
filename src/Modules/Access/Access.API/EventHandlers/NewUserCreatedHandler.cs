using Access.API.Events;
using Access.API.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Access.API.EventHandlers
{
    public class NewUserCreatedHandler : INotificationHandler<NewUserCreatedEvent>
    {
        private readonly ILogger<ForgotPasswordEvent> _logger;
        private readonly IEmailService _emailService;

        public NewUserCreatedHandler(ILogger<ForgotPasswordEvent> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }
        public Task Handle(NewUserCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending user creation notification to {notification.Person.Email}");

            var body = $@"<p>Hello {notification.Person.FirstName},</p> 
                    <p>You have been registered on the MyStar platform Stella Maris Schools</p> 
                    <p>Here is your password to login {notification.password}</p>";

            return _emailService.SendEmailAsync(notification.Person.Email, "Welcome to MyStar Platform", body);
        }
    }
}
