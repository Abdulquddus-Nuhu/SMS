using System;
using Module.Access.Models.Request;
using Module.Access.Models.Response;

namespace Module.Access.Services.Interfaces
{
    public interface IPersonaService
    {
        public Task<ApiResponse<ParentResponse>> CreateParentAsync(CreateParentRequest request, string host);
        public Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request, string host);
        public Task<BaseResponse> CreateBusDriverAsync(CreateBusDriverRequest request, string host);
        public Task<BaseResponse> CreateStaffAsync(CreateStaffRequest request, string host);
    }
}

