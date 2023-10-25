using System;
namespace Module.Access.Services.Interfaces
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}

