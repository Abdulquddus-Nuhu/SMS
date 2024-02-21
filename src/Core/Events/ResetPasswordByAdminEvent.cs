using MediatR;
using Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Events
{
    public record ResetPasswordByAdminEvent(PersonaResponse Person, string newPassword) : INotification;

}
