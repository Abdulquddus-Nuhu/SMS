using System;
using Microsoft.AspNetCore.Identity;

namespace Module.Access.Identity
{
    public class Persona : IdentityUser<Guid>
    {
        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public bool IsDeleted { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
        public virtual DateTime Created { get; protected set; }
        public virtual DateTime? Modified { get; protected set; }
        public virtual string? LastModifiedBy { get; protected set; }
        public string? PhotoUrl { get; set; }
        public int PesonaType { get; set; }

        //Staff
        public string? JobTitle { get; set; } = string.Empty;
        public string? Department { get; set; } = string.Empty;

        //Student
        public Guid? ParentId { get; set; }
        public string? Grade { get; set; } = string.Empty;
        public bool? BusServiceRequired { get; set; }

        //Bus driver
        public string BusNumber { get; set; } = string.Empty;

        public Persona(DateTime created, bool isDeleted)
        {
            Created = created;
            IsDeleted = isDeleted;
            Id = Guid.NewGuid();
        }
        public Persona() : this(DateTime.UtcNow, false) { }
    }

}

