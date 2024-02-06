using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
{
    public interface IDepartmentService
    {
        public Task<BaseResponse> CreateDepartment(CreateDepartmentRequest request, string creator);
        public Task<ApiResponse<List<DepartmentResponse>>> GetAllAsync();
    }
}
