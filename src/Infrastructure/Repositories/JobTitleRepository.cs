using Core.Entities;
using Core.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using static Shared.Constants.StringConstants;

namespace Infrastructure.Repositories
{
    public class JobTitleRepository : IJobTitleRepository
    {
        private readonly AppDbContext _dbContext;
        public JobTitleRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<BaseResponse> AddJobTitle(JobTitle JobTitle)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            _dbContext.JobTitles.Add(JobTitle);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to create JobTitle! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;
        }
        public async Task<IQueryable<JobTitle>> GetAllAsync()
        {
            return _dbContext.JobTitles.AsQueryable().AsNoTracking();
        }


    }
}
