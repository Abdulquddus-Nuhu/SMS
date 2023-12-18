using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Interfaces;
using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Access.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;

namespace Access.API.Services.Implementation
{
    public class BusService : IBusService
    {
        private readonly IBusRepository _busRepository;
        public BusService(IBusRepository busRepository)
        {
            _busRepository = busRepository;
        }

        public async Task<BaseResponse> CreatBus(CreateBusRequest request, string creator)
        {
            var bus = new Bus()
            {
                Number = request.BusNumber,
                CreatedBy = creator,
                Created = DateTime.UtcNow,
            };

            var result = await _busRepository.AddBus(bus);
            return result;
        }

        public async Task<ApiResponse<List<BusResponse>>> GetAllAsync()
        {
            var buses = await _busRepository.GetAllAsync();

            return new ApiResponse<List<BusResponse>>()
            {
                Data = await buses.Select(x => new BusResponse()
                {
                    Id = x.Id,
                    BusNumber = x.Number
                }).ToListAsync()
            };
        }
    }
}
