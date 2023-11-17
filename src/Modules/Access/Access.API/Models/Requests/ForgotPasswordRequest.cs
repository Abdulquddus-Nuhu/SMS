using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Requests
{
    public record ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Please provide a value for Email Address field")]
        [StringLength(30)]
        public string Email { get; init; } = string.Empty;
    }
}
