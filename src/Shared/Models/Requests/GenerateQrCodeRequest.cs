using Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace Models.Requests
{
    public class GenerateQrCodeRequest
    {
        public string UserEmail { get; set; } = string.Empty;
        [Required]
        public Guid StudentId { get; set; }
    }
}
