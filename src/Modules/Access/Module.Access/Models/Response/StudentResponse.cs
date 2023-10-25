using System;
namespace Module.Access.Models.Response
{
    public record StudentResponse
    {
        public string StudentId { get; set; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public bool BusServiceRequired { get; set; }
        public string Role { get; set; } = string.Empty;
    }
}

