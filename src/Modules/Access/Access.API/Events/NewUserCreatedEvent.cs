using Access.API.Models.Responses;
using MediatR;

namespace Access.API.Events
{
    public record NewUserCreatedEvent(PersonaResponse Person, string password) : INotification;
}
