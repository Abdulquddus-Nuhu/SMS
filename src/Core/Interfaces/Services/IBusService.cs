using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Interfaces.Services
{
    public interface IBusService
    {
        public Task<BaseResponse> CreatBus(CreateBusRequest request, string creator);
        public Task<ApiResponse<List<BusResponse>>> GetAllAsync();
    }
}
