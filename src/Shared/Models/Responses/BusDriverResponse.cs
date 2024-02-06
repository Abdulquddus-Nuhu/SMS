using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Responses
{
    public record BusDriverResponse
    {
        public Guid BusDriverId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; init; } = string.Empty;
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;
        public string BusNumber { get; set; } = string.Empty;

    }

}
