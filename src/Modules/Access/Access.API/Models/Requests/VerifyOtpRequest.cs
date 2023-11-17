using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Access.Models.Requests
{
    public record VerifyOtpRequest
    {
        [Required(ErrorMessage = "Please provide your email address for login")]
        [StringLength(30)]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide OTP")]
        //[StringLength(4, MinimumLength = 4)]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; init; } = string.Empty;
    }
}
