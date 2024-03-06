﻿using Core.Entities;
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

        public Task UpdateAsync(Trip trip)
        {
            throw new NotImplementedException();
        }
        public Task DeleteAsync(Trip trip)
        {
            throw new NotImplementedException();
        }

    }
}
