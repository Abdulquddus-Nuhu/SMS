using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Responses
{
    public record LoginResponse
    {
        public bool Result { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public DateTime TokenExpiryDate { get; set; }
        public string Email { get; set; } = string.Empty;
        public bool IsLockedOut { get; set; } = false;
        public string PhotoUrl { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public IList<string>? Roles { get; set; }
    }

}
