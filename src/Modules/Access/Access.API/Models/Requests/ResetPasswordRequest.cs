using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Requests
{
    public record ResetPasswordRequest
    {
        //[Required(ErrorMessage = "Please provide a value for Email Address field"), RegularExpression(StringConstants.EMAIL_REGEX, ErrorMessage = "The Email field is not a valid e-mail address.")]
        [StringLength(255)]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for the Token field")]
        [StringLength(255)]
        public string Token { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for password field"), MinLength(8, ErrorMessage = "Password must consist of at least 8 characters")]
        [StringLength(255)]
        public string NewPassword { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for the confirm password field"), Compare(nameof(NewPassword), ErrorMessage = "Passwords do not match")]
        [StringLength(255)]
        public string ConfirmPassword { get; init; } = string.Empty;
    }

}
