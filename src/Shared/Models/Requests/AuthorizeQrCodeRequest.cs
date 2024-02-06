using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Requests
{
    public record AuthorizeQrCodeRequest
    {
        public Guid QrCodeId { get; set; }
        [Required]
        public AuthorizedUserType AuthorizedUser { get; set; }

        public string? AuthorizedUserRelationship { get; set; }
        public string? AuthorizedUserFirstName { get; set; }
        public string? AuthorizedUserLastName { get; set; }
    }
}
