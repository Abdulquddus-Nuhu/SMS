using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.Models.Requests;
using Access.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IPersonaService
    {
        public Task<ApiResponse<ParentResponse>> CreateParentAsync(CreateParentRequest request, string host);
        public Task<ApiResponse<List<ParentResponse>>> ParentListAsync();
        public Task<BaseResponse> EditParentAsync(Guid parentId, EditParentRequest request, string editor);
        public Task<ApiResponse<ParentResponse>> GetParentAsync(Guid parentId);
        public Task<BaseResponse> DeleteParentAsync(Guid parentId, string deletor);
        public Task<ApiResponse<StudentResponse>> CreateStudentAsync(CreateStudentRequest request, string host);
        public Task<ApiResponse<List<StudentResponse>>> StudentListAsync();
        public Task<ApiResponse<StudentResponse>> GetStudentAsync(Guid studentId);
        public Task<BaseResponse> DeleteStudnetAsync(Guid studentId, string deletor);
        public Task<ApiResponse<List<StudentResponse>>> ParentStudentsListAsync(Guid parentId);
        public Task<BaseResponse> CreateBusDriverAsync(CreateBusDriverRequest request, string host);
        public Task<BaseResponse> CreateStaffAsync(CreateStaffRequest request, string host);
        public Task<ApiResponse<List<StaffResponse>>> StaffListAsync();
        public Task<ApiResponse<List<BusDriverResponse>>> BusDriverListAsync();

        public Task<ApiResponse<StudentResponse>> EditStudentAsync(Guid studentId, EditStudentRequest request, string editor);

        public Task<ApiResponse<List<PersonaResponse>>> GetUsersWithRole();
        public Task<BaseResponse> DeletePersonaAccountAsync(string email);
    }
}
