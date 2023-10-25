using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Access.Models.Request
{
    public class CreateStudentRequest
    {
        [Required]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        public string LastName { get; init; } = string.Empty;

        [Required]
        public string Grade { get; init; } = string.Empty;

        [Required]
        public bool BusServiceRequired { get; init; }

        [Required]
        public IFormFile Photo { get; set; }

        [Required]
        public string ParentId { get; set; } = string.Empty;
    }
}

