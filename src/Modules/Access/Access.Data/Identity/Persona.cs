using Microsoft.AspNetCore.Identity;
using Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Data.Identity
{
    public class Persona : BaseIdentity
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string? PhotoUrl { get; set; }
        public PersonaType PesonaType { get; set; }
        public bool IsActive { get; set; } = true;

        //Staff
        public string? JobTitle { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;

        //Student
        public Guid? ParentId { get; set; }
        public string? Grade { get; set; } = string.Empty;
        public bool? BusServiceRequired { get; set; }

        //Bus driver
        public string? BusNumber { get; set; } = string.Empty;

        public Persona(DateTime created, bool isDeleted)
        {
            Created = created;
            IsDeleted = isDeleted;
            Id = Guid.NewGuid();
        }
        public Persona() : this(DateTime.UtcNow, false) { }

        public void Delete(string deletor)
        {
            IsDeleted = true;
            Deleted = DateTime.UtcNow;
            DeletedBy = deletor;
        }
    }

}
