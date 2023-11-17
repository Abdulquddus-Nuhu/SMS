using Access.Models.Requests;
using Access.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IPersonaService
    {
        public Task<ApiResponse<ParentResponse>> CreateParentAsync(CreateParentRequest request, string host);
        public Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request, string host);
        public Task<BaseResponse> CreateBusDriverAsync(CreateBusDriverRequest request, string host);
        public Task<BaseResponse> CreateStaffAsync(CreateStaffRequest request, string host);
        public Task<ApiResponse<List<ParentResponse>>> ParentListAsync();
        public Task<ApiResponse<List<StudentResponse>>> StudentListAsync();
        public Task<ApiResponse<List<StudentResponse>>> ParentStudentsListAsync(Guid parentId);
        public Task<ApiResponse<List<StaffResponse>>> StaffListAsync();
        public Task<ApiResponse<List<BusDriverResponse>>> BusDriverListAsync();
    }
}
