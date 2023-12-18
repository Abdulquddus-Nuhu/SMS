using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

namespace Access.Data.Repositories
{
    public class BusRepository : IBusRepository
    {
        private readonly AccessDbContext _dbContext;
        public BusRepository(AccessDbContext dbContext)
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
