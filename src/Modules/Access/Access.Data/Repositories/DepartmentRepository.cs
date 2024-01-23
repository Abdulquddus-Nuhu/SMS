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
    public class DepartmentRepository : IDepartmentRepository
    {
        private readonly AccessDbContext _dbContext;
        public DepartmentRepository(AccessDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<BaseResponse> AddDepartment(Department department)
        {
            var response = new BaseResponse() { Code = ResponseCodes.Status201Created };

            _dbContext.Departments.Add(department);

            var result = await _dbContext.TrySaveChangesAsync();
            if (result)
            {
                return response;
            }

            response.Message = "Unable to create department! Please try again";
            response.Status = false;
            response.Code = ResponseCodes.Status500InternalServerError;

            return response;

        }

        public async Task<IQueryable<Department>> GetAllAsync()
        {
            return _dbContext.Departments.AsQueryable().AsNoTracking();
        }
    }
}
