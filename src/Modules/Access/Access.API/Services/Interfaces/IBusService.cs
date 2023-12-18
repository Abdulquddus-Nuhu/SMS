using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Shared.Models.Responses;

namespace Access.API.Services.Interfaces
{
    public interface IBusService
    {
        public Task<BaseResponse> CreatBus(CreateBusRequest request, string creator);
        public Task<ApiResponse<List<BusResponse>>> GetAllAsync();
    }
}
