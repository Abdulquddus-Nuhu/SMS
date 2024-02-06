using Core.Entities;
using Shared.Models.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Repositories
{
    public interface ICampusRepository
    {
        public Task<BaseResponse> AddCampus(Campus campus);
        public Task<IQueryable<Campus>> GetAllAsync();
        public Task<IQueryable<Campus>> GetCampusWithGradesAsync();
    }
}
