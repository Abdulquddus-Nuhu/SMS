using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities.Users
{
    public class Student : BaseEntity
    {
        //UserId or PersonaId
        public Guid PersonaId { get; set; }

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string? PhotoUrl { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Sex { get; set; }
        public string? Nationality { get; set; }
        public string? State { get; set; }
        public string? FatherNationality { get; set; }
        public string? MotherNationality { get; set; }
        public string? Religion { get; set; }
        public string? MotherTongue { get; set; }
        public DateTime? DateOfEntry { get; set; }

        /// <summary>
        /// class for which admission is sought
        /// </summary>
        public string? AdmissionSoughtClass { get; set; }
        public string? Class { get; set; }
        public string? ContactAddress { get; set; }
        public string? PermanentAddress { get; set; }
        public string? LanguagesSpoken { get; set; }

        /// <summary>
        /// details of schools attended (most recent first) such as class/grade ,dates attended the school in month & year
        /// </summary>
        public string? SchoolsAttended { get; set; }

        /// <summary>
        /// Additional information such as any history of health problems ,allergies ,asthma, e.t.c
        /// </summary>
        public string? HealthProblems { get; set; }

        public string? Email { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; } = string.Empty;
        public Guid GradeId { get; set; }
        public Grade? Grade { get; set; }
        public bool? BusServiceRequired { get; set; }
        public Guid? BusId { get; set; }
        public Bus? Bus { get; set; }

        //public Guid ParentId { get; set; }
        //public Parent? Parent { get; set; }
        public List<Parent>? Parents { get; set; }

        public ICollection<TripStudent> TripStudents { get; set; } = new List<TripStudent>();

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
