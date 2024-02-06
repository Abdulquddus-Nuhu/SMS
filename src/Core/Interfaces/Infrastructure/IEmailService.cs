namespace Core.Interfaces.Infrastructure
{
    public interface IEmailService
    {
        public Task SendEmailAsync(string email, string subject, string htmlMessage);
    }
}
