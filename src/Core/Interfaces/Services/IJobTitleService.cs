using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
{
    public interface IJobTitleService
    {
        public Task<BaseResponse> CreateJobTitle(CreateJobTitleRequest request);
        public Task<ApiResponse<List<JobTitleRespone>>> GetAllAsync();


    }
}
 