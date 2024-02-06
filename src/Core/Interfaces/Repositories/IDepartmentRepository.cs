using Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IDepartmentRepository
    {
        public Task<BaseResponse> AddDepartment(Department department);
        public Task<IQueryable<Department>> GetAllAsync();
    }
}
