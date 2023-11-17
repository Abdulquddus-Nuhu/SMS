using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Requests
{
    public record ChangePasswordRequest
    {
        [Required(ErrorMessage = "Please provide your old password")]
        [StringLength(255)]
        public string OldPassword { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for the password field"), MinLength(8, ErrorMessage = "Password must consist of at least 6 characters")]
        [StringLength(255)]
        public string NewPassword { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for the Confirm Password field"), Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        [StringLength(255)]
        public string ConfirmNewPassword { get; init; } = string.Empty;
    }
}
