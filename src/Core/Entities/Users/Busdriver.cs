using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Users
{
    public class Busdriver : BaseEntity
    {
        //UserId or PersonaId
        public Guid PersonaId { get; set; }

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string? PhotoUrl { get; set; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;

        public Guid? BusId { get; set; }
        public Bus? Bus { get; set; }

        public void Delete(string deletor)
        {
            IsDeleted = true;
            Deleted = DateTime.UtcNow;
            DeletedBy = deletor;
        }
    }
}
