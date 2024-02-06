using Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IGradeRepository
    {
        public Task<BaseResponse> AddGrade(Grade grade);
        public Task<IQueryable<Grade>> GetAllAsync();

        
    }
}
