using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Models.Responses
{
    public record BaseResponse
    {
        public int Code { get; set; } = 0;
        public bool Status { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}
