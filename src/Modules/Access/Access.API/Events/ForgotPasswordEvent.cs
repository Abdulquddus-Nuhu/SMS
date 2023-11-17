using Access.API.Models.Responses;
using MediatR;

namespace Access.API.Events
{
    public record ForgotPasswordEvent(PersonaResponse Person, string Otp) : INotification;
}
