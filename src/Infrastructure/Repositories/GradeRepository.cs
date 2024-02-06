using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AppDbContext _dbContext;
        public GradeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> AddGrade(Grade grade)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created};

            _dbContext.Grades.Add(grade);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to create grade! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }

        public async Task<IQueryable<Grade>> GetAllAsync()
        {
            return _dbContext.Grades.Include(x => x.Campus).AsQueryable().AsNoTracking();
        }
    }
}
