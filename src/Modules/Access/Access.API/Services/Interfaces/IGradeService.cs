using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IGradeService
    {
        public Task<BaseResponse> CreateGrade(CreateGradeRequest request);
       
        public Task<ApiResponse<List<GradeResponse>>> GetAllAsync();
    }
}
