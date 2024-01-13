using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Requests
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
    }

}
