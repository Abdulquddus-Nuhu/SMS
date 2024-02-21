using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Requests
{
    public record ResetPasswordByAdminRequest
    {
        [Required(ErrorMessage = "Please provide a value for Email Address field")]
        [StringLength(255)]
        public string Email { get; init; } = string.Empty;


        [Required(ErrorMessage = "Please provide a value for password field"), MinLength(8, ErrorMessage = "Password must consist of at least 8 characters")]
        [StringLength(255)]
        public string NewPassword { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for the confirm password field"), Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        [StringLength(255)]
        public string ConfirmPassword { get; init; } = string.Empty;
    }
}
