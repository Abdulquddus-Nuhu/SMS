using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Requests
{
    public record RemoveStudentFromTripRequest
    {
        public Guid TripId { get; set; }
        public Guid StudentId { get; set; }
    }
}
