using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IJobTitleService
    {
        public Task<BaseResponse> CreateJobTitle(CreateJobTitleRequest request);
        public Task<ApiResponse<List<JobTitleRespone>>> GetAllAsync();


    }
}
 