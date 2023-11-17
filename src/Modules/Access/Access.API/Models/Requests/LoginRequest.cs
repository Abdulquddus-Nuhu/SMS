using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Requests
{
    public record LoginRequest
    {
        //[Required(ErrorMessage = "Please provide a value for Email Address field"), RegularExpression(StringConstants.EMAIL_REGEX, ErrorMessage = "The Email field is not a valid e-mail address.")]
        [StringLength(255)]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide a value for password field")]
        [StringLength(255)]
        public string Password { get; init; } = string.Empty;
    }
}
