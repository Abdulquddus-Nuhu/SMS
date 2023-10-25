using System;
namespace Module.Access.Models.Response
{
    public record BaseResponse
    {
        public int Code { get; set; } = 0;
        public bool Status { get; set; } = true;
        public string Message { get; set; } = string.Empty;
    }
}

