using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly AppDbContext _dbContext;
        public BusRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> AddBus(Bus bus)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            _dbContext.Buses.Add(bus);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to create bus! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;

        }

        public async Task<IQueryable<Bus>> GetAllAsync()
        {
            return _dbContext.Buses.AsQueryable().AsNoTracking();
        }
    }
}
