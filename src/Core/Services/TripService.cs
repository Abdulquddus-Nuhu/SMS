using Core.Entities;
using Core.Entities.Users;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Models.Responses;
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
        private readonly IStudentRepository _studentRepository;
        private readonly IBusDriverRepository _busDriverRepository;
        private readonly ILogger<TripService> _logger;
        public TripService(ITripRepository tripRepository, IStudentRepository studentRepository,
            IBusDriverRepository busDriverRepository, ILogger<TripService> logger)
        {
            _tripRepository = tripRepository;
            _studentRepository = studentRepository;
            _busDriverRepository = busDriverRepository;
            _logger = logger;
        }

        public async Task<ApiResponse<Guid?>> CreateTripAsync(CreateTripRequest request, string driver)
        {
            var response = new ApiResponse<Guid?>();

            var trip = new Trip
            {
                Id = Guid.NewGuid(),
                BusDriver = driver,
                FuelCousumption = request.FuelConsumption,
                IsReFuel = request.IsRefuel,
                ReasonForReFuel = request.ReasonForRefuel,
                RouteFollowed = request.RouteFollowed,
                TripType = request.TripType,
            };

            var result = await _tripRepository.AddAsync(trip);
            if (!result.Status)
            {
                response.Data = null;
                response.Code = result.Code;
                response.Message = result.Message;
                response.Status = result.Status;
            }

            response.Data = trip.Id;
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

        public async Task<BaseResponse> AddStudentToTripAsync(AddStudentToTripRequest request)
        {
            var tripStudent = new TripStudent
            {
                Id = Guid.NewGuid(),
                TripId = request.TripId,
                StudentId = request.StudentId,
                EnrollmentDate = DateTime.UtcNow
            };

            return await _tripRepository.AddStudentToTripAsync(tripStudent);
        }

        public async Task<ApiResponse<IEnumerable<StudentResponse>>> GetNotOnboardedStudentAsync(Guid tripId, string busDriverEmail)
        {
            var busDriver = await _busDriverRepository.GetBusdriverByEmail(busDriverEmail);
            if (busDriver is null)
            {
                _logger.LogInformation("Bus driver not found when trying to get not onboarded student.");
                return new ApiResponse<IEnumerable<StudentResponse>>()
                {
                    Data = Enumerable.Empty<StudentResponse>()
                };
            }

            var allStudents = await _studentRepository.GetStudentsWithBusServiceAsync(busDriver.BusId.Value).ToListAsync();
            var tripStudents = await _tripRepository.GetTripStudentsByTripId(tripId).ToListAsync();


            var onboardedStudentIds = tripStudents.Select(ts => ts.StudentId).ToHashSet();
            var notOnboardedStudents = allStudents.Where(s => !onboardedStudentIds.Contains(s.Id));


            return new ApiResponse<IEnumerable<StudentResponse>>()
            {
                Data = notOnboardedStudents.Select(x => new StudentResponse()
                {
                    StudentId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhotoUrl = x.PhotoUrl,
                    Grade = x.Grade.Name,
                })
            };

        }

        public async Task<ApiResponse<IEnumerable<StudentResponse>>> GetOnboardedStudentAsync(Guid tripId, string busDriverEmail)
        {
            var busDriver = await _busDriverRepository.GetBusdriverByEmail(busDriverEmail);
            if (busDriver is null)
            {
                _logger.LogInformation("Bus driver not found when trying to get not onboarded student.");
                return new ApiResponse<IEnumerable<StudentResponse>>()
                {
                    Data = Enumerable.Empty<StudentResponse>()
                };
            }

            var allStudents = await _studentRepository.GetStudentsWithBusServiceAsync(busDriver.BusId.Value).ToListAsync();
            var tripStudents = await _tripRepository.GetTripStudentsByTripId(tripId).ToListAsync();


            var onboardedStudentIds = tripStudents.Select(ts => ts.StudentId).ToHashSet();
            var onboardedStudents = allStudents.Where(s => onboardedStudentIds.Contains(s.Id));


            return new ApiResponse<IEnumerable<StudentResponse>>()
            {
                Data = onboardedStudents.Select(x => new StudentResponse()
                {
                    StudentId = x.Id,
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    PhotoUrl = x.PhotoUrl,
                    Grade = x.Grade.Name,
                })
            };

        }

 
        public async Task<BaseResponse> RemoveStudentFromTripAsync(RemoveStudentFromTripRequest request)
        {
            return await _tripRepository.RemoveStudentFromTripAsync(request.TripId, request.StudentId);
        }
    }
}
