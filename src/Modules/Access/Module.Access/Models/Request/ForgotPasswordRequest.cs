using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Access.Models.Request
{
    public class ForgotPasswordRequest
    {
        [Required(ErrorMessage = "Please provide a value for Email Address field")]
        [StringLength(255)]
        public string Email { get; init; } = string.Empty;
    }
}

