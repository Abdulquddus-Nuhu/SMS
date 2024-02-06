using MediatR;
using Models.Responses;

namespace Core.Events
{
    public record ForgotPasswordEvent(PersonaResponse Person, string Otp) : INotification;
}
