using Core.Entities;
using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
using Models.Requests;
using Models.Responses;
using Shared.Models.Responses;

namespace Core.Services
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepository;
        public CampusService(ICampusRepository campusRepository)
        {
            _campusRepository = campusRepository;
        }

        public async Task<BaseResponse> CreateCampus(CreateCampusRequest request, string creator)
        {
            var campus = new Campus()
            {
                Name = request.Name,
                Location = request.Location,
                CreatedBy = creator,
                Created = DateTime.UtcNow,
            };

            var result = await _campusRepository.AddCampus(campus);
            return result;
        }

        public async Task<ApiResponse<List<CampusResponse>>> GetAllAsync()
        {
            var campuses = await _campusRepository.GetAllAsync();

            return new ApiResponse<List<CampusResponse>>()
            {
                Data = await campuses.Select(x => new CampusResponse()
                {
                    Id = x.Id,
                    Name = x.Name,
                    Location = x.Location,
                }).ToListAsync() 
            };
        }
        
        public async Task<ApiResponse<List<CampusResponse>>> GetCampusWithGradesAsync()
        {
            var campuses = await _campusRepository.GetCampusWithGradesAsync();

            return new ApiResponse<List<CampusResponse>>()
            {
                Data = await campuses.Select(c => new CampusResponse()
                {
                    Id = c.Id,
                    Name = c.Name,
                    Location = c.Location,
                    Grades = c.Grades
                    .Select(g => new GradeResponse()
                    {
                        Id = g.Id,
                        Name = g.Name,
                        Arms = g.Arms,
                        Level = g.Level,
                        CampusName = c.Name
                    })
                    .ToList(),

                }).ToListAsync() 
            };
        }

    }
}
