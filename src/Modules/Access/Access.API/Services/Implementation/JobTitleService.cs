using Access.API.Models.Requests;
using Access.API.Models.Responses;
using Access.API.Services.Interfaces;
using Access.Core.Entities;
using Access.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Shared.Models.Responses;

namespace Access.API.Services.Implementation
{
    public class JobTitleService : IJobTitleService
    {
        private readonly IJobTitleRepository _jobTitleRepository;

        public JobTitleService(IJobTitleRepository jobTitleRepository)
        {
            _jobTitleRepository = jobTitleRepository;
        }

        public async Task<BaseResponse> CreateJobTitle(CreateJobTitleRequest request)
        {
            var jobTitle = new JobTitle()
            {
                Name = request.Name,
                
          
            };

            var result = await _jobTitleRepository.AddJobTitle(jobTitle);
            return result;
        }

        // Add additional methods as needed for job title-related operations

        public async Task<ApiResponse<List<JobTitleRespone>>> GetAllAsync()
        {
            var jobTitle = await _jobTitleRepository.GetAllAsync();
            return new ApiResponse<List<JobTitleRespone>>()
            {
                Data = await jobTitle.Select(x => new JobTitleRespone()
                {
                    Id = x.Id,
                    Name = x.Name,
                }).ToListAsync()
            };
        }
    }


}
