using MediatR;
using Models.Responses;

namespace Core.Events
{
    public record NewUserCreatedEvent(PersonaResponse Person, string password) : INotification;
}
