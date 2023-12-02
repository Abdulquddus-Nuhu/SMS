﻿using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Interfaces;
using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;

namespace Access.API.Services.Implementation
{
    public class CampusService : ICampusService
    {
        private readonly ICampusRepository _campusRepository;
        public CampusService(ICampusRepository campusRepository)
        {
                _campusRepository = campusRepository;
        }

        public async Task<BaseResponse> CreateCampus(CreateCampusRequest request)
        {
            var campus = new Campus()
            {
                Name = request.Name,
                Location = request.Location,
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

    }
}
