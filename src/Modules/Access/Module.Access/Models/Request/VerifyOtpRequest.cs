using System;
using System.ComponentModel.DataAnnotations;

namespace Module.Access.Models.Request
{
    public record VerifyOtpRequest
    {
        [Required(ErrorMessage = "Please provide your email address for login")]
        [StringLength(255)]
        public string Email { get; init; } = string.Empty;

        [Required(ErrorMessage = "Please provide OTP")]
        [StringLength(4, MinimumLength = 4)]
        public string Otp { get; init; } = string.Empty;
    }
}

