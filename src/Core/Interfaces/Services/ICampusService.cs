using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
{
    public interface ICampusService
    {
        public Task<BaseResponse> CreateCampus(CreateCampusRequest request, string creator);
        public Task<ApiResponse<List<CampusResponse>>> GetAllAsync();
        public Task<ApiResponse<List<CampusResponse>>> GetCampusWithGradesAsync();
    }
}
