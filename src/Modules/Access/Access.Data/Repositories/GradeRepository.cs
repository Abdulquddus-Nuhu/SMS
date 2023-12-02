using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Shared.Constants.StringConstants;

namespace Access.Data.Repositories
{
    public class GradeRepository : IGradeRepository
    {
        private readonly AccessDbContext _dbContext;
        public GradeRepository(AccessDbContext dbContext)
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
