using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Interfaces;
using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;

namespace Access.API.Services.Implementation
{
    public class GradeService : IGradeService
    {
        private readonly IGradeRepository _gradeRepository;
        public GradeService(IGradeRepository gradeRepository)
        {
            _gradeRepository = gradeRepository;
        }

        public async Task<BaseResponse> CreateGrade(CreateGradeRequest request)
        {
            var grade = new Grade()
            {
                Name = request.Name,
                Arms = request.Arms,
                CampusId = request.CampusId,
                Level = request.Level,
            };

            var result = await _gradeRepository.AddGrade(grade);
            return result;
        }

        public async Task<ApiResponse<List<GradeResponse>>> GetAllAsync()
        {
            var grades = await _gradeRepository.GetAllAsync();

            return new ApiResponse<List<GradeResponse>>()
            {
                Data = await grades.Select(x => new GradeResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Arms = x.Arms,
                    CampusName = x.Campus!.Name,
                    Level = x.Level
                }).ToListAsync()
            };
        }
    }
}
