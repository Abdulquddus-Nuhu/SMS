using Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ITripRepository
    {
        public Task<Trip?> GetByIdAsync(Guid id);
        public IQueryable<Trip> GetAllAsync();
        public Task<BaseResponse> AddAsync(Trip trip);
        public Task<BaseResponse> AddStudentToTripAsync(TripStudent tripStudent);
        public Task<BaseResponse> RemoveStudentFromTripAsync(Guid tripId, Guid studentId);
        public Task UpdateAsync(Trip trip);
        public Task DeleteAsync(Trip trip);
        public IQueryable<TripStudent> GetTripStudentsByTripId(Guid tripId);
        Task GetTripStudentAsync(Guid tripId, Guid studentId);
    }
}
