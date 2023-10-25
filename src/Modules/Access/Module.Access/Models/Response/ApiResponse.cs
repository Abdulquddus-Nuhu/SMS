using System;
namespace Module.Access.Models.Response
{
    public record ApiResponse<TData> : BaseResponse
    {
        public TData? Data { get; set; }
    }
}

