using Shared.Enums;

namespace Core.Entities.Users
{
    public class Parent : BaseEntity
    {
        //UserId or PersonaId
        public Guid PersonaId { get; set; }

        public string LastName { get; set; } = string.Empty;
        public string FirstName { get; set; } = string.Empty;
        public string FullName { get => $"{FirstName} {LastName}"; }
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string? PhotoUrl { get; set; }

        public string? HomeAddress { get; set; }
        public string? PostalAddress { get; set; }
        public string? Occupation { get; set; }
        public string? NameOfEmployer { get; set; }
        public string? BuisnessAddress { get; set; }
        public string? BuisnessPostalAddress { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }

        public List<Student>? Students { get; set; }


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
