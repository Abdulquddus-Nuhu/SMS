using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Core.Entities.Users
{
    public class Student : BaseEntity
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string? PhotoUrl { get; set; }

        public Guid ParentId { get; set; }
        public Parent? Parent { get; set; }
        public Guid GradeId { get; set; }
        public Grade? Grade { get; set; }
        public bool? BusServiceRequired { get; set; }

        public void Delete(string deletor)
        {
            IsDeleted = true;
            Deleted = DateTime.UtcNow;
            DeletedBy = deletor;
        }
    }
}
