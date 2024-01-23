using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IDepartmentService
    {
        public Task<BaseResponse> CreateDepartment(CreateDepartmentRequest request, string creator);
        public Task<ApiResponse<List<DepartmentResponse>>> GetAllAsync();
    }
}
