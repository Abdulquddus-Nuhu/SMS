using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class TripRepository : ITripRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<TripRepository> _logger;
        public TripRepository(AppDbContext dbContext, ILogger<TripRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<BaseResponse> AddAsync(Trip trip)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            _dbContext.Trips.Add(trip);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to create new trip! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }

        public IQueryable<Trip> GetAllAsync()
        {
            return _dbContext.Trips.AsQueryable().AsNoTracking();
        }

        public async Task<Trip?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Trips.FirstOrDefaultAsync(t => t.Id == id);
        }

        public IQueryable<TripStudent> GetTripStudentsByTripId(Guid tripId)
        {
            return _dbContext.TripStudents
                                 .Where(ts => ts.TripId == tripId)
                                 //.Include(ts => ts.Student) // Optionally include Student details
                                 .AsNoTracking();
        }

        public async Task<BaseResponse> AddStudentToTripAsync(TripStudent tripStudent)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            await _dbContext.TripStudents.AddAsync(tripStudent);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to add student to the trip! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }

        public async Task<BaseResponse> RemoveStudentFromTripAsync(Guid tripId, Guid studentId)
        {
            var response = new BaseResponse();

            var tripStudent = await _dbContext.TripStudents
                .FirstOrDefaultAsync(ts => ts.TripId == tripId && ts.StudentId == studentId);

            if (tripStudent != null)
            {
                _logger.LogInformation($"Removing student {studentId} from trip {tripId}");

                _dbContext.TripStudents.Remove(tripStudent);

                if (!await _dbContext.TrySaveChangesAsync())
                {
                    _logger.LogInformation($"Unable to remove student {studentId} from trip {tripId}");
                    response.Status = false;
                    response.Message = "Unable to remove student from the trip! Please try again";
                    response.Code = ResponseCodes.Status500InternalServerError;
                    return response;
                }
            }

            return response;
        }


        public Task UpdateAsync(Trip trip)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(Trip trip)
        {
            throw new NotImplementedException();
        }

        public Task GetTripStudentAsync(Guid tripId, Guid studentId)
        {
            throw new NotImplementedException();
        }

    }
}
