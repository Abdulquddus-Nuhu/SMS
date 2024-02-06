using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Responses
{
    public record StudentResponse
    {
        public Guid StudentId { get; set; }
        public string FirstName { get; init; } = string.Empty;
        public string LastName { get; init; } = string.Empty;
        public string PhotoUrl { get; set; } = string.Empty;
        public string Grade { get; set; } = string.Empty;
        public bool? BusServiceRequired { get; set; }
        public string Role { get; set; } = string.Empty;
    }

}
