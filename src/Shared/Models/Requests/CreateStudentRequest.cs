using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Requests
{
    public class CreateStudentRequest
    {
        [Required]
        public string FirstName { get; init; } = string.Empty;

        [Required]
        public string LastName { get; init; } = string.Empty;

        [Required]
        public Guid GradeId { get; init; }

        [Required]
        public bool BusServiceRequired { get; init; }
        public Guid? BusId { get; set; }

        [Required]
        public IFormFile? Photo { get; set; }

        [Required]
        public Guid ParentId { get; set; }

        [Required(ErrorMessage = "Please provide a value for password field"), MinLength(8, ErrorMessage = "Password must consist of at least 8 characters")]
        [StringLength(255)]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for the confirm password field"), Compare(nameof(Password), ErrorMessage = "Passwords do not match")]
        [StringLength(255)]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

}
