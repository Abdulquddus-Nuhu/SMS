using Shared.Enums;

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
        //public string? JobTitle { get; set; } = string.Empty;
        //public Guid? JobTitleId { get; set; }
        //public JobTitle? JobTitle { get; set; }
        //public string? Department { get; set; } = string.Empty;
        //public Guid? DepartmentId { get; set; } 
        //public Department? Department { get; set; }

        //Student
        //public Guid? ParentId { get; set; }
        //public string? Grade { get; set; } = string.Empty;
        //public Guid? GradeId { get; set; }
        //public Grade? Grade { get; set; }
        //public bool? BusServiceRequired { get; set; }


        //Bus driver
        //public string? BusNumber { get; set; } = string.Empty;
        //public Guid? BusId { get; set; }
        //public Bus? Bus { get; set; }

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

        public void Edit(string editor)
        {
            Modified = DateTime.UtcNow;
            LastModifiedBy = editor;
        }
    }

}
