using Core.Events;
using Core.Interfaces.Infrastructure;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.EventHandlers
{
    public class NewStudentCreatedHandler : INotificationHandler<NewStudentCreatedEvent>
    {
        private readonly ILogger<ForgotPasswordEvent> _logger;
        private readonly IEmailService _emailService;

        public NewStudentCreatedHandler(ILogger<ForgotPasswordEvent> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }
        public Task Handle(NewStudentCreatedEvent notification, CancellationToken cancellationToken)
        {
            _logger.LogInformation($"Sending student credentials to parent: {notification.parent},");

            var body = $@"<p>Hello {notification.parent},</p> 
                    <p>Your child have been registered on the MyStar platform Stella Maris Schools</p> 
                    <p>Here is your child's email to login {notification.Person.Email}</p>
                    <p>Here is your child's password to login {notification.password}</p>";

            return _emailService.SendEmailAsync(notification.Person.Email, "Welcome to MyStar Platform", body);
        }

    }

}
