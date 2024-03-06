using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Requests
{
    public record CreateTripRequest
    {
        public string? FuelConsumption { get; set; }
        public bool IsRefuel { get; set; }
        public string? ReasonForRefuel { get; set; }
        public string? RouteFollowed { get; set; }
        public DateTime? LastMaintenanceDate { get; set; }
        public TripType? TripType { get; set; }
    }
}
