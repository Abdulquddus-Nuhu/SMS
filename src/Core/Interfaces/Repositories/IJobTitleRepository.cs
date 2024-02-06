using Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface IJobTitleRepository
    {
        public Task<BaseResponse> AddJobTitle(JobTitle JobTitle );
        public Task<IQueryable<JobTitle>> GetAllAsync();

    }
}
