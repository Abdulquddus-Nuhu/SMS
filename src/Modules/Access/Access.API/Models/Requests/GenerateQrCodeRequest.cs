using Access.Core.Entities.Users;
using Sharedd.Enums;

namespace Access.API.Models.Requests
{
    public class GenerateQrCodeRequest
    {
        public Guid StudentId { get; set; }
        public AuthorizedUserType AuthorizedUser { get; set; }
        public string? AuthorizedUserRelationship { get; set; }
        public string? AuthorizedUserFirstName { get; set; }
        public string? AuthorizedUserLastName { get; set; }
    }
}
