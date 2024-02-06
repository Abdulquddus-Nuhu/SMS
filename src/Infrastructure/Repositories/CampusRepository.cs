using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class CampusRepository : ICampusRepository
    {
        private readonly AppDbContext _dbContext;
        public CampusRepository(AppDbContext dbContext) 
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> AddCampus(Campus campus)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            _dbContext.Campuses.Add(campus);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result) 
            {
                return response;
            }

            response.Message = "Unable to create campus! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }

        public async Task<IQueryable<Campus>> GetAllAsync()
        {
            return _dbContext.Campuses.AsQueryable().AsNoTracking();
        }
        
        public async Task<IQueryable<Campus>> GetCampusWithGradesAsync()
        {
            return _dbContext.Campuses.Include(x => x.Grades).AsQueryable().AsNoTracking();
        }

        public async Task<Campus> GetByIdAsync(Guid id)
        {
            return await _dbContext.Set<Campus>().FindAsync(id);
        }

        public void Remove(Campus campus)
        {
            _dbContext.Set<Campus>().Remove(campus);
        }

        public void Update(Campus campus)
        {
            _dbContext.Set<Campus>().Update(campus);
        }
    }
}
