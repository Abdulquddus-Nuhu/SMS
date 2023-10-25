using System;
namespace Module.Access.Models.Response
{
    public record LoginResponse
    {
        public bool Result { get; set; } = false;
        public string Token { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public bool IsLockedOut { get; set; } = false;
        public string PhotoUrl { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public IList<string>? Roles { get; set; }
    }
}

