using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface ICampusService
    {
        public Task<BaseResponse> CreateCampus(CreateCampusRequest request, string creator);
        public Task<ApiResponse<List<CampusResponse>>> GetAllAsync();
        public Task<ApiResponse<List<CampusResponse>>> GetCampusWithGradesAsync();
    }
}
