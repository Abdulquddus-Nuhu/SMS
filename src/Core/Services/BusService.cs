using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Services
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
                NumberOfSeat = request.NumberOfSeat,
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
                    NumberOfSeat = x.NumberOfSeat,
                    BusNumber = x.Number
                }).ToListAsync()
            };
        }
    }
}
