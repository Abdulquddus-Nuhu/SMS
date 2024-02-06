using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class EditStudentRequest
    {
        public string? FirstName { get; set; }

        public string? LastName { get; set; }

        public Guid? GradeId { get; set; }

        public bool? BusServiceRequired { get; set; }

       
    }
}
