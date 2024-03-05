using Models.Requests;
using Models.Responses;
using Shared.Models.Requests;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
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
        public Task<BaseResponse> UpdateParentInfoAsync(Guid parentId, UpdateParentInfoRequest request, string editor);
        public Task<BaseResponse> DeleteUserByAdminAsync(Guid userId);
    }
}
