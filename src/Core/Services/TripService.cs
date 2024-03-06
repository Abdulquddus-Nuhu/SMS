using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Requests;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class TripService : ITripService
    {
        private readonly ITripRepository _tripRepository;
        public TripService(ITripRepository tripRepository)
        {
            _tripRepository = tripRepository;
        }

        public async Task<BaseResponse> CreateTripAsync(CreateTripRequest request, string driver)
        {
            var response = new BaseResponse();

            var trip = new Trip
            {
                BusDriver = driver,
                FuelCousumption = request.FuelConsumption,
                IsReFuel = request.IsRefuel,
                ReasonForReFuel = request.ReasonForRefuel,
                RouteFollowed = request.RouteFollowed,
                TripType = request.TripType,
            };

            response = await _tripRepository.AddAsync(trip);
            return response;
        }

        public async Task<ApiResponse<List<TripResponse>>> TripListAsync()
        {
            var trips =  _tripRepository.GetAllAsync();

            return new ApiResponse<List<TripResponse>>()
            {
                Data = await trips.Select(x => new TripResponse()
                {
                    Id = x.Id,
                    DateCreated = x.Created,
                    EndTime = x.EndTime,
                    StartTime = x.StartTime,
                    TripType = x.TripType,
                }).ToListAsync()
            };
        }
    }
}
