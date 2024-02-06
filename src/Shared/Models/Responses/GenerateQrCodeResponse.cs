using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Responses
{
    public record GenerateQrCodeResponse
    {
        public Guid QrCodeId { get; set; }
        public string QrCodeData { get; set; } = string.Empty;
    }
}
