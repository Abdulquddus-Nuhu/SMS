using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class GenerateQrCodeRequest
    {
        //[Required]
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        public Guid StudentId { get; set; }
        [Required]
        public AuthorizedUserType AuthorizedUser { get; set; }

        public string? AuthorizedUserRelationship { get; set; }
        public string? AuthorizedUserFirstName { get; set; }
        public string? AuthorizedUserLastName { get; set; }
    }
}
