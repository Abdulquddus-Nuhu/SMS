using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
{
    public interface IGradeService
    {
        public Task<BaseResponse> CreateGrade(CreateGradeRequest request);
       
        public Task<ApiResponse<List<GradeResponse>>> GetAllAsync();
    }
}
