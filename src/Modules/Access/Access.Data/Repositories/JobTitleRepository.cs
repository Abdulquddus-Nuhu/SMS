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
   public class JobTitleRepository : IJobTitleRepository
    {
        private readonly AccessDbContext _dbContext;
        public JobTitleRepository(AccessDbContext dbContext)
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
