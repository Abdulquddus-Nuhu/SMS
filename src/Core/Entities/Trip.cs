using Core.Entities.Users;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class Trip : BaseEntity
    {
        [StringLength(100)]
        public string BusDriver { get; set; } = string.Empty;

        [StringLength(100)]
        public string FuelCousumption { get; set; } = string.Empty;
        public bool IsReFuel { get; set; }
        public string? ReasonForReFuel { get; set; } = string.Empty;

        public string? RouteFollowed { get; set; } = string.Empty;
        public DateTime? LastMaintenanceDate { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public TripType? TripType { get; set;}

        public ICollection<TripStudent> TripStudents { get; set; } = new List<TripStudent>();

    }
}
